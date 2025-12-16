using ApiContract;
using Entities;
using gRPCRepositories;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieRepository movieRepository;

    public MovieController(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository;
    }

    /// <summary>
    /// get all movies, with optional search query
    /// </summary>
    /// <remarks>
    /// If a query is provided, it filters movies whose titles contain the query string (case-insensitive).
    /// </remarks>
    /// <param name="query"></param>
    /// <returns></returns>
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
            m.Genre,
            m.description,
            m.poster_Url,
            m.banner_Url
        )));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var movie = await movieRepository.GetSingleAsync(id);
            return Ok(new MovieDto(movie.DurationMinutes, movie.MovieId, movie.ReleaseDate, movie.MovieTitle,
                movie.Genre, movie.description, movie.poster_Url, movie.banner_Url));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] MovieCreateDto dto)
    {
        var movie = new Movie
        {
            MovieTitle = dto.MovieTitle,
            Genre = dto.Genre,
            DurationMinutes = dto.DurationMinutes,
            ReleaseDate = dto.ReleaseDate,
            description = dto.Description,
            poster_Url = dto.PosterUrl,
            banner_Url = dto.BannerUrl
        };

        var saved = await movieRepository.AddAsync(movie);

        return Ok(saved);
    }
}