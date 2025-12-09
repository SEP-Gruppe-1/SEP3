using ApiContract;
using Grpc.Core;
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
            return Ok(new ScreenDto(screening.screeningId, screening.movie, screening.hall, screening.hallId,
                screening.hall.Seats, screening.startTime,
                screening.date, screening.availableSeats));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{screeningId:int}/Book")]
    public async Task<IActionResult> BookSeats(int screeningId, [FromBody] BookSeatsRequest request)
    {

        try
        {


            await screeningRepository.BookSeatsAsync(
                screeningId,
                request.SeatIds,
                request.PhoneNumber
            );

            return Ok(new { message = "Seats booked successfully" });
        }
        catch (RpcException ex)
        {
            if (ex.StatusCode == Grpc.Core.StatusCode.Internal &&
                ex.Status.Detail.Contains("Seats already booked"))
            {
                return BadRequest(new
                {
                    error = "One or more seats are already booked.",
                    details = ex.Status.Detail
                });
            }

            // fallback -> ukendt serverfejl
            return StatusCode(500, new
            {
                error = "Unexpected server error.",
                details = ex.Status.Detail
            });
        }
    }
    
}