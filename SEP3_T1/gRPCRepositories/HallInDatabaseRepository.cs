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

    public Task<Hall> getHallbyidAsync(int id)
    {
        var hall = halls.SingleOrDefault(h => h.Id == id);
        if (hall == null)
            throw new InvalidOperationException($"Hall with id nr: {id} not found.");
        return Task.FromResult(hall);
    }

    public IQueryable<Hall> GetAll()
    {
        return halls.AsQueryable();
    }

    public async Task Initialize()
    {
        halls = client.GetHallsAsync().Result;
    }
}