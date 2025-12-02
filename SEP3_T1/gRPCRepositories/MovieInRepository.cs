using Entities;
using RepositoryContracts;

namespace gRPCRepositories;

public class MovieInRepository : IMovieRepository
{
    private readonly CinemaServiceClient _client;
    private List<Movie> movies;

    public MovieInRepository(CinemaServiceClient _client)
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

    public async Task<Movie?> GetSingleAsync(int id)
    {
        var all = await _client.GetMoviesAsync();
        return all.SingleOrDefault(m => m.MovieId == id)
               ?? throw new InvalidOperationException();
    }

    public IQueryable<Movie> GetAll()
    {
        return movies.AsQueryable();
    }

    public async Task InitializeAsync()
    {
        movies = await _client.GetMoviesAsync();
    }
}