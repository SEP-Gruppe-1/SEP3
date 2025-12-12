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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await movieRepository.GetAllAsync();
        return Ok(movies);
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