using System.Globalization;
using ApiContract;
using Entities;
using Grpccinema;
using RepositoryContracts;
using ApiContract;

namespace gRPCRepositories;

public class ScreeningInDatabaseRepository : IScreeningRepository
{
    private readonly CinemaServiceClient _client;
    private List<Screening> screenings;

    public ScreeningInDatabaseRepository(CinemaServiceClient _client)
    {
        this._client = _client;
        screenings = new List<Screening>();
    }

    private static TimeOnly ParseFlexibleTime(string input)
    {
        var normalized = input.Trim().Replace('.', ':');

        if (TimeOnly.TryParseExact(
                normalized,
                new[] { "HH:mm", "HH:mm:ss" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var time))
        {
            return time;
        }

        throw new FormatException($"Invalid time format: '{input}'");
    }

    public async Task<Screening> AddAsync(ScreeningCreateDto dto)
    {
        await _client.GetLayoutsAsync();
        // Hent movie og hall via gRPC
        var movieDto = await _client.getMovieById(dto.MovieId);
        var hallDto = await _client.GetHallByIdAsync(dto.HallId);

        var movie = new Movie
        {
            MovieId = movieDto.Id,
            MovieTitle = movieDto.Title,
            DurationMinutes = movieDto.Playtime,
            Genre = movieDto.Genre,
            ReleaseDate = movieDto.ReleaseDate
        };

        var hall = Hall.GetInstance(hallDto.Id);

        var startTime = ParseFlexibleTime(dto.StartTime);


        var date = DateOnly.ParseExact(
            dto.Date,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture
        );

        var screening = new Screening
        {
            movie = movie,
            hall = hall,
            hallId = hall.Id,
            startTime = startTime,
            date = date,
            availableSeats = hall.Seats.Count
        };

        return await _client.SaveScreeningAsync(screening);
    }


    public async Task updateAsync(Screening screening)
    {
        var savedScreening = await _client.SaveScreeningAsync(screening);


        var existing = screenings.SingleOrDefault(s => s.screeningId == savedScreening.screeningId);
        if (existing != null) screenings.Remove(existing);

        screenings.Add(savedScreening);
    }

    public Task deleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Screening?> getSingleAsync(int id)
    {
        var screening = (await _client.GetScreeningsAsync()).First(s => s.screeningId == id);

        var seats = await _client.GetSeatsByScreeningIdAsync(id);

        screening.hall.Seats = seats;

        return screening;
    }

    public async Task<List<Screening>> getAll()
    {
        var screenings = await _client.GetScreeningsAsync();

        foreach (var s in screenings)
        {
            var seats = await _client.GetSeatsByScreeningIdAsync(s.screeningId);
        }


        return screenings;
    }

 
    public Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber)
    {
        return _client.BookSeatsAsync(screeningId, seatIds, phoneNumber);
    }

    public async Task UpdateBookingAsync(int screeningId, string phoneNumber, List<int> seatsToAdd, List<int> seatsToRemove)
    {
        await _client.UpdateBookingAsync(screeningId, phoneNumber, seatsToAdd, seatsToRemove);
    }
    public async Task<List<CustomerBookingDto>> GetBookingsByPhoneAsync(string phone)
    {
        var result = new List<CustomerBookingDto>();

        // Hent alle screenings fra Java
        var allScreenings = await _client.GetScreeningsAsync();

        foreach (var s in allScreenings)
        {
            // Hent sæder til den pågældende screening
            var seats = await _client.GetSeatsByScreeningIdAsync(s.screeningId);

            // Find sæder booket af denne kunde
            var bookedByCustomer = seats
                .Where(seat =>
                    seat.IsBooked &&
                    seat.Customer != null &&
                    seat.Customer.Phone == phone)
                .ToList();

            if (!bookedByCustomer.Any())
                continue;

            var seatLabels = bookedByCustomer
                .Select(seat => $"{seat.Row}{seat.Number}")
                .ToList();

            var seatIds = bookedByCustomer
                .Select(seat => seat.id)
                .ToList();

            result.Add(new CustomerBookingDto(
                ScreeningId: s.screeningId,
                MovieTitle: s.movie.MovieTitle,
                Date: s.date.ToString("yyyy-MM-dd"),
                Time: s.startTime.ToString("HH:mm"),
                Seats: seatLabels,
                SeatIds: seatIds
            ));
        }

        return result;
    }
    
    public async Task DeleteBookingAsync(int screeningId, string phone)
    {
        var seats = await _client.GetSeatsByScreeningIdAsync(screeningId);

        var booked = seats
            .Where(s => s.Customer?.Phone == phone)
            .ToList();

        if (!booked.Any())
            return;

        // Unbook seats by sending empty phone
        await _client.DeleteBookingAsync(
           screeningId,phone
        );
    }


    
    
 
    
    

}