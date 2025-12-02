using ApiContract;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ScreeningController : ControllerBase
{
    private readonly IScreeningRepository screeningRepository;
    
    public ScreeningController(IScreeningRepository screeningRepository)
    {
        this.screeningRepository = screeningRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllScreenings()
    {
        var screenings = await screeningRepository.getAll();
        return Ok(screenings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScreenDto>> GetSingle(int id)
    {
        try
        {
            var screening = await screeningRepository.getSingleAsync(id);
            return Ok(new ScreenDto( screening.screeningId,screening.movie, screening.hallId, screening.startTime, screening.date, screening.availableSeats));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }


}