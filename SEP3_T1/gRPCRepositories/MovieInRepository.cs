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

    public Task<Movie> AddAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie?> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Movie> GetAll()
    {
        throw new NotImplementedException();
    }
}