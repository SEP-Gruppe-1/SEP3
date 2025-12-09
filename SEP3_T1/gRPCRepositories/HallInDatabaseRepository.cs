using Entities;
using RepositoryContract;

namespace gRPCRepositories;

public class HallInDatabaseRepository : IHallRepository
{
    private readonly CinemaServiceClient client;
    private List<Hall> halls;

    public HallInDatabaseRepository(CinemaServiceClient client)
    {
        this.client = client;
        halls = new List<Hall>();
    }

    public List<Seat> GetSelectedSeats()
    {
        throw new NotImplementedException();
    }

    public List<Seat> GetAvailableSeats()
    {
        throw new NotImplementedException();
    }

    public string GetBookedSeatsDisplay()
    {
        throw new NotImplementedException();
    }

    public Seat GetSeat(char row, int number)
    {
        throw new NotImplementedException();
    }

    public async Task<Hall> getHallbyidAsync(int id)
    {
        if (!halls.Any())
            await Initialize();

        var hall = halls.SingleOrDefault(h => h.Id == id);
        if (hall == null)
            throw new InvalidOperationException($"Hall with id nr: {id} not found.");
        return hall;

        // var hall = halls.SingleOrDefault(h => h.Id == id);
        // if (hall == null)
        //     throw new InvalidOperationException($"Hall with id nr: {id} not found.");
        // return Task.FromResult(hall);
    }

    public IQueryable<Hall> GetAll()
    {
        return halls.AsQueryable();
    }


    public List<Seat> GetSelectedSeats(int hallId)
    {
        var hall = halls.Single(h => h.Id == hallId);
        return hall.Seats.Where(s => s.IsBooked).ToList();
    }

    public List<Seat> GetAvailableSeats(int hallId)
    {
        var hall = halls.Single(h => h.Id == hallId);
        return hall.Seats.Where(s => !s.IsBooked).ToList();
    }

    public string GetBookedSeatsDisplay(int hallId)
    {
        var hall = halls.Single(h => h.Id == hallId);
        return string.Join(", ", hall.Seats.Where(s => s.IsBooked)
            .Select(s => $"{s.Row}{s.Number}"));
    }

    public Seat GetSeat(int hallId, char row, int number)
    {
        var hall = halls.Single(h => h.Id == hallId);
        return hall.Seats.Single(s => s.Row == row && s.Number == number);
    }

    public async Task Initialize()
    {
        halls = client.GetHallsAsync().Result;
    }
}