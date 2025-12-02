using ApiContract;
using gRPCRepositories;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class MovieController :ControllerBase
{
    private readonly IMovieRepository movieRepository;
    
    public MovieController (IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository;
    }

    [HttpGet("{int id}")]
    public async Task<ActionResult<MovieDto>> GetSingle(int id)
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

    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        if (movieRepository is MovieInRepository repo)
            await repo.InitializeAsync();
        
        var movies = movieRepository.GetAll().ToList();
        return Ok(movies);
    }
}