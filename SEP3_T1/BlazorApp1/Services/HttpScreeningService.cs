using System.Net;
using System.Net.Http.Json;
using ApiContract;

namespace BlazorApp1.Services;

public class HttpScreeningService : IScreeningService
{
    private readonly HttpClient http;
    private readonly JwtHttpClientHandler jwtHandler;

    public HttpScreeningService(HttpClient http, JwtHttpClientHandler jwtHandler)
    {
        this.http = http;
        this.jwtHandler = jwtHandler;
    }
    
    /// <summary>
    /// Get bookings for customer by phone
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public async Task<List<CustomerBookingDto>> GetBookingsForCustomerAsync(string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.GetAsync($"api/screening/bookings/{phone}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Kunne ikke hente bookinger.");
        }

        return await response.Content.ReadFromJsonAsync<List<CustomerBookingDto>>() 
               ?? new List<CustomerBookingDto>();
    }
    
    /// <summary>
    /// Delete booking seat
    /// </summary>
    /// <param name="screeningId"></param>
    /// <param name="phone"></param>
    /// <exception cref="Exception"></exception>

    public async Task DeleteBookingSeatAsync(int screeningId, string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.DeleteAsync($"api/screening/{screeningId}/booking/{phone}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Kunne ikke slette booking.");
        }
    }
    
    /// <summary>
    /// Not implemented delete booking
    /// </summary>
    /// <param name="bookingToDelete"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public Task DeleteBookingAsync(CustomerBookingDto bookingToDelete)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// get bookings by phone
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public async Task<List<CustomerBookingDto>> GetBookingsByPhoneAsync(string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.GetAsync($"api/customer/bookings/{phone}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Kunne ikke hente bookinger");

        return await response.Content.ReadFromJsonAsync<List<CustomerBookingDto>>() 
               ?? new List<CustomerBookingDto>();
    }
    
    /// <summary>
    /// get screening by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ScreenDto> GetScreeningAsync(int id)
    {
        await jwtHandler.AttachJwtAsync(http);
        return await http.GetFromJsonAsync<ScreenDto>($"api/screening/{id}")
               ?? throw new Exception("Could not load screening.");
    }
    
    /// <summary>
    /// Create screening
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="Exception"></exception>

    public async Task CreateScreeningAsync(ScreeningCreateDto dto)
    {
        
        await jwtHandler.AttachJwtAsync(http);
        var response = await http.PostAsJsonAsync("api/Screening", dto);

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var errorObj = await response.Content.ReadFromJsonAsync<HttpCustomerService.ConflictResponse>();
            var msg = errorObj?.Message ?? "Telefonnummer eller email er allerede registreret.";
            throw new Exception(msg);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error adding Customer: {responseContent}");
    
    }
    
    /// <summary>
    /// Book seats for screening
    /// </summary>
    /// <param name="screeningId"></param>
    /// <param name="seatIds"></param>
    /// <param name="phone"></param>
    /// <exception cref="Exception"></exception>

    public async Task BookSeatsAsync(int screeningId, List<int> seatIds, string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var request = new
        {
            SeatIds = seatIds,
            PhoneNumber = phone
        };

        var response = await http.PostAsJsonAsync(
            $"api/screening/{screeningId}/book",
            request
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Kunne ikke booke sæder: {error}");
        }
    }
}