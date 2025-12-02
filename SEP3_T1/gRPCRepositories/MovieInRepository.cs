using Entities;
using Grpccinema;
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
    
    public async Task InitializeAsync()
    {
        movies = await _client.GetMoviesAsync();
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
        var movie = movies.SingleOrDefault(m => m.MovieId == id);
        if (movie == null)
            throw new InvalidOperationException($"Movie with ID: {id} not found.");
        return Task.FromResult(movie);
    }

    public IQueryable<Movie> GetAll()
    {
        return movies.AsQueryable();
    }
}