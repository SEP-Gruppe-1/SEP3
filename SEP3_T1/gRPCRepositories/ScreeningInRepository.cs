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
    
}/*
    public async Task Initialize()
    {
       screenings = await _client.GetScreeningsAsync();
    }
}*/