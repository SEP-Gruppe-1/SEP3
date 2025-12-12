using ApiContract;

namespace BlazorApp1.Services;

public interface IScreeningService
{
    Task<List<CustomerBookingDto>> GetBookingsForCustomerAsync(string phone);
    Task DeleteBookingAsync(int screeningId, string phone);
    Task DeleteBookingAsync(CustomerBookingDto bookingToDelete);
    Task<List<CustomerBookingDto>> GetBookingsByPhoneAsync(string phone);
    Task<ScreenDto> GetScreeningAsync(int id);
    Task BookSeatsAsync(int screeningId, List<int> seatIds, string phone);
    


}