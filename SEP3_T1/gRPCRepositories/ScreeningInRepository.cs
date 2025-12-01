using Entities;
using Grpccinema;
using RepositoryContracts;

namespace gRPCRepositories;

public class ScreeningInRepository : IScreeningRepository
{
    private List<Screening> screenings;
    private readonly CinemaService.CinemaServiceClient _client;
    
    public ScreeningInRepository(CinemaService.CinemaServiceClient _client)
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

    public Task<Screening?> getSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Screening> getAll()
    {
        throw new NotImplementedException();
    }
}/*
    public async Task Initialize()
    {
       screenings = await _client.GetScreeningsAsync();
    }
}*/