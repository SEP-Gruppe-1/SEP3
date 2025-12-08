using Entities;
using Grpccinema;
using RepositoryContracts;

namespace gRPCRepositories;

public class ScreeningInRepository : IScreeningRepository
{
    private readonly CinemaServiceClient _client;
    private List<Screening> screenings;

    public ScreeningInRepository(CinemaServiceClient _client)
    {
        this._client = _client;
        screenings = new List<Screening>();
    }

    public Task<Screening> AddAsync(Screening screening)
    {
        throw new NotImplementedException();
    }

    public Task updateAsync(Screening screening)
    {
        throw new NotImplementedException();
    }

    public Task deleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Screening?> getSingleAsync(int id)
    {
        var screening  = (await _client.GetScreeningsAsync()).First(s => s.screeningId == id);
        
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
            s.hall.Seats = seats;
        }
        
        
        return screenings;
    }

 
    public Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber)
    {
        return _client.BookSeatsAsync(screeningId, seatIds, phoneNumber);
    }
}