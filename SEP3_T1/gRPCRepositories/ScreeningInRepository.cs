using Entities;
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
        var all = await _client.GetScreeningsAsync();
        return all.SingleOrDefault(s => s.screeningId == id)
               ?? throw new InvalidOperationException();
    }

    public async Task<List<Screening>> getAll()
    {
        return await _client.GetScreeningsAsync();
    }
}