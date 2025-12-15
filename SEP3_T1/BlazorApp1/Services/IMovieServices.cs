using ApiContract;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(int id);

    Task<List<MovieDto>> SearchMoviesAsync(string query);
    Task CreateMovieAsync(MovieCreateDto dto);
}