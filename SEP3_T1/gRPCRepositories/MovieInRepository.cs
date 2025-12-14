using Entities;
using gRPCRepositories;
using RepositoryContracts;

namespace gRPCRepositories;

public class MovieInRepository : IMovieRepository
{
    private readonly CinemaServiceClient client;
    private List<Movie> movies = new();

    public MovieInRepository(CinemaServiceClient client)
    {
        this.client = client;
    }

    public async Task<Movie?> GetSingleAsync(int id)
    {
        return (await client.GetMoviesAsync())
               .SingleOrDefault(m => m.MovieId == id)
               ?? throw new InvalidOperationException($"Movie {id} not found");
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        movies = await client.GetMoviesAsync();
        return movies;
    }
    
    public async Task<Movie> AddAsync(Movie movie)
    {
        
        movie.MovieId = 0;

        var saved = await _client.SaveMovieAsync(movie);
        
        movies.Add(saved);

        return saved;
    }

    public Task UpdateAsync(Movie movie)
        => throw new NotImplementedException();

    public Task DeleteAsync(int id)
        => throw new NotImplementedException();
}