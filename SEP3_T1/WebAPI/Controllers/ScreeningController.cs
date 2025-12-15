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
            var s = await screeningRepository.getSingleAsync(id);

            var dto = new ScreenDto(
                s.screeningId,
                new MovieDto(
                    s.movie.DurationMinutes,
                    s.movie.MovieId,
                    s.movie.ReleaseDate,
                    s.movie.MovieTitle,
                    s.movie.Genre,
                    s.movie.description,
                    s.movie.poster_Url,
                    s.movie.banner_Url
                ),
                new HallDto(
                    s.hall.Id,
                    s.hall.Number,
                    s.hall.LayoutId
                ),
                s.hallId,
                s.hall.Seats.Select(seat => new SeatDto(
                    seat.id,
                    seat.Row,
                    seat.Number,
                    seat.IsBooked,
                    seat.Customer?.Phone
                )).ToList(),
                s.startTime,
                s.date,
                s.availableSeats
            );

            return Ok(dto);
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
                return BadRequest(new
                {
                    error = "One or more seats are already booked.",
                    details = ex.Status.Detail
                });

           
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

    [HttpGet("bookings/{phone}")]
    public async Task<IActionResult> GetBookingsForCustomer(string phone)
    {
        var bookings = await screeningRepository.GetBookingsByPhoneAsync(phone);
        return Ok(bookings);
    }
    
    [HttpDelete("{screeningId:int}/booking/{phone}")]
    public async Task<IActionResult> DeleteBooking(int screeningId, string phone)
    {
        try
        {
            await screeningRepository.DeleteBookingAsync(screeningId, phone);
            return Ok(new { message = "Booking deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("book")]
    public async Task<IActionResult> BookSeats([FromBody] BookingRequest request)
    {
        await screeningRepository.BookSeatsAsync(request.ScreeningId, request.SeatIds, request.Phone);
        return Ok();
    }

    public class BookingRequest
    {
        public int ScreeningId { get; set; }
        public List<int> SeatIds { get; set; }
        public string Phone { get; set; }
    }

}