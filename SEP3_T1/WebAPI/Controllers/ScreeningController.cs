using ApiContract;
using Grpc.Core;
using Grpccinema;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using BookSeatsRequest = ApiContract.BookSeatsRequest;

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

    [HttpPost]
    public async Task<ActionResult<ScreenDto>> AddScreening([FromBody] ScreeningCreateDto request)
    {
        var screening = await screeningRepository.AddAsync(request);
        return Ok(screening);
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

           
            return StatusCode(500, new
            {
                error = "Unexpected server error.",
                details = ex.Status.Detail
            });
        }
    }

     [HttpPut("{screeningId:int}/UpdateBooking")]
     public async Task<IActionResult> UpdateBooking(int screeningId, [FromBody] UpdateBookingDTO dto)
     {
            await screeningRepository.UpdateBookingAsync(
                screeningId,
                dto.CustomerPhone,
                dto.SeatsToAdd,
                dto.SeatsToRemove
            );
            var result = new
            { message = "Booking updated successfully"
            };

              return Ok(result);
        
     }

}