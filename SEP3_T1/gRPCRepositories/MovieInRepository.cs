using Entities;
using Grpccinema;
using RepositoryContracts;

namespace gRPCRepositories;

public class MovieInRepository : IMovieRepository
{
    private readonly CinemaService.CinemaServiceClient _client;
    private List<Movie> movies;

    public MovieInRepository(CinemaService.CinemaServiceClient _client)
    {
        this._client = _client;
        movies = new List<Movie>();
    }
    
}