using ApiContract;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieRepository movieRepository;

    public MovieController(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository;
    }

    // ✅ Ét samlet endpoint
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? query)
    {
        var movies = await movieRepository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(query))
        {
            movies = movies
                .Where(m => m.MovieTitle.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Ok(movies.Select(m => new MovieDto(
            m.DurationMinutes,
            m.MovieId,
            m.ReleaseDate,
            m.MovieTitle,
            m.Genre
        )));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var movie = await movieRepository.GetSingleAsync(id);
            return Ok(movie);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}