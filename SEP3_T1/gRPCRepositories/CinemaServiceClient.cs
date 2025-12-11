using Entities;
using Grpc.Net.Client;
using Grpccinema;
using BookSeatsRequest = Grpccinema.BookSeatsRequest;

namespace gRPCRepositories;

public class CinemaServiceClient
{
    private readonly CinemaService.CinemaServiceClient client;

    public CinemaServiceClient(string serverAddress)
    {
        var channel = GrpcChannel.ForAddress(serverAddress);
        client = new CinemaService.CinemaServiceClient(channel);
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var response = await client.GetCustomersAsync(new GetCustomersRequest());
        var customers = new List<Customer>();
        foreach (var dtoCustomer in response.Customers)
            customers.Add(new Customer
            {
                Phone = dtoCustomer.Phone,
                Name = dtoCustomer.Name,
                Email = dtoCustomer.Email,
                Password = dtoCustomer.Password,
                Role = dtoCustomer.Role
            });

        return await Task.FromResult(customers);
    }

    public async Task<DTOCustomer> GetCustomerByPhoneAsync(string phone)
    {
        var response = await client.GetCustomerByPhoneAsync(new GetCustomerByPhoneRequest { Phone = phone });
        return response.Customer;
    }


    public async Task<Customer> SaveCustomerAsync(Customer customer)
    {
        var dto = new DTOCustomer
        {
            Name = customer.Name,
            Email = customer.Email,
            Password = customer.Password,
            Phone = customer.Phone,
            Role = customer.Role
        };

        var request = new SaveCustomerRequest { Customer = dto };

        var response = await client.SaveCustomerAsync(request);


        return new Customer
        {
            Name = response.Customer.Name,
            Email = response.Customer.Email,
            Password = response.Customer.Password,
            Phone = response.Customer.Phone,
            Role = response.Customer.Role
        };
    }

    public async Task DeleteCustomerAsync(string phone)
    {
        var request = new DeleteCustomerRequest
        {
            Phone = phone
        };

        await client.DeleteCustomerAsync(request);
    }

    public async Task<bool> VerifyCustomerPasswordAsync(string phone, string password)
    {
        var request = new VerifyCustomerPasswordRequest
        {
            Phone = phone,
            Password = password
        };
        var response = await client.VerifyCustomerPasswordAsync(request);
        return response.IsValid;
    }


    public async Task<List<Hall>> GetHallsAsync()
    {
        var response = await client.GetHallsAsync(new GetHallsRequest());
        Console.WriteLine("gRPC returned hall count: " + response.Halls.Count);

        var halls = new List<Hall>();

        foreach (var dto in response.Halls)
        {
            Console.WriteLine($"Hall from gRPC: {dto.Id}, {dto.Number}, {dto.Layout}");

            var hall = Hall.GetInstance(dto.Id);
            hall.Number = dto.Number;
            hall.LayoutId = dto.Layout;
            hall.Id = dto.Id;
        }

        return await Task.FromResult(halls);
    }

    public async Task<Hall> GetHallByIdAsync(int id)
    {
        var response = await client.GetHallByIDAsync(new GetHallByIdRequest { Id = id });
        var dto = response.Hall;


        var hall = Hall.GetInstance(id);


        hall.Number = dto.Number;
        hall.LayoutId = dto.Layout;
        hall.Id = dto.Id;

        return hall;
    }

    public async Task<List<Screening>> GetScreeningsAsync()
    {
        await GetLayoutsAsync();
        var response = await client.GetAllScreeningsAsync(new GetAllScreeningsRequest());
        var screenings = new List<Screening>();

        foreach (var dtoScreening in response.Screenings)
        {
            var dtoMovie = await getMovieById(dtoScreening.Movie.Id);
            var movie = ConvertToMovie(dtoMovie);

            var timeOnly = TimeOnly.Parse(dtoScreening.StartTime);
            var dateOnly = DateOnly.Parse(dtoScreening.Date);
            screenings.Add(new Screening
            {
                movie = movie,
                screeningId = dtoScreening.Id,
                hall = ConvertToHall(dtoScreening.Hall),
                hallId = dtoScreening.HallId,
                startTime = timeOnly,
                date = dateOnly,
                availableSeats = dtoScreening.AvailableSeats
            });
        }

        return await Task.FromResult(screenings);
    }

    private Movie ConvertToMovie(DTOMovie dto)
    {
        return new Movie
        {
            MovieId = dto.Id,
            MovieTitle = dto.Title,
            Genre = dto.Genre,
            DurationMinutes = dto.Playtime,
            ReleaseDate = dto.ReleaseDate
        };
    }

    private Hall ConvertToHall(DTOHall dto)
    {
        var hall = Hall.GetInstance(dto.Id);
        hall.Number = dto.Number;
        hall.LayoutId = dto.Layout;
        hall.Id = dto.Id;
        return hall;
    }


    private Customer ConvertToCustomer(DTOCustomer dto)
    {
        if (dto == null)
            return null;

        return new Customer
        {
            Phone = dto.Phone,
            Name = dto.Name,
            Email = dto.Email
        };
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var response = await client.GetAllMoviesAsync(new GetAllMoviesRequest());
        var movies = new List<Movie>();
        foreach (var dtoMovie in response.Movies)
            movies.Add(new Movie
            {
                MovieId = dtoMovie.Id,
                MovieTitle = dtoMovie.Title,
                DurationMinutes = dtoMovie.Playtime,
                Genre = dtoMovie.Genre,
                ReleaseDate = dtoMovie.ReleaseDate
            });
        return await Task.FromResult(movies);
    }

    public async Task<DTOMovie> getMovieById(int id)
    {
        var response = await client.GetMovieByIDAsync(new GetMovieByIdRequest { Id = id });
        return response.Movie;
    }


    public async Task<List<Layout>> GetLayoutsAsync()
    {
        var response = await client.GetAllLayoutsAsync(new GetAllLayoutsRequest());
        var layouts = new List<Layout>();
        foreach (var dtoLayout in response.Layouts)
        {
            var layout = Layout.Create(dtoLayout.Id, dtoLayout.MaxLetter[0], dtoLayout.MaxSeatInt);


            layout.maxLetter = dtoLayout.MaxLetter[0];
            layout.maxSeatInt = dtoLayout.MaxSeatInt;
            layout.id = dtoLayout.Id;
            layouts.Add(layout);
        }

        return await Task.FromResult(layouts);
    }

    public async Task<List<Seat>> GetSeatsAsync()
    {
        var response = await client.GetAllSeatsAsync(new GetAllSeatsRequest());
        var seats = new List<Seat>();
        foreach (var dtoSeat in response.Seats)
        {
            var seat = new Seat
            {
                id = dtoSeat.Id,
                Row = dtoSeat.Letter[0],
                Number = dtoSeat.Number,
                IsBooked = dtoSeat.Booked,
                Customer = ConvertToCustomer(dtoSeat.Customer)
            };
            seats.Add(seat);
        }

        return await Task.FromResult(seats);
    }

    public async Task<List<Seat>> GetSeatsByScreeningIdAsync(int screeningId)
    {
        var response =
            await client.GetSeatsByScreeningAsync(new GetSeatsByScreeningRequest { ScreeningId = screeningId });
        var seats = new List<Seat>();
        foreach (var dtoSeat in response.Seats)
        {
            var seat = new Seat
            {
                id = dtoSeat.Id,
                Row = dtoSeat.Letter[0],
                Number = dtoSeat.Number,
                IsBooked = dtoSeat.Booked,
                Customer = dtoSeat.Customer != null ? ConvertToCustomer(dtoSeat.Customer) : null
            };
            seats.Add(seat);
        }

        return await Task.FromResult(seats);
    }

    public async Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber)
    {
        var request = new BookSeatsRequest
        {
            ScreeningId = screeningId,
            CustomerPhone = phoneNumber
        };

        request.SeatIds.AddRange(seatIds);

        await client.BookSeatsAsync(request);
    }
}