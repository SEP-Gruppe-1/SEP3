using ApiContract;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(int id);
}