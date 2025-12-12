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

    public async Task DeleteBookingAsync(int screeningId, string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.DeleteAsync($"api/screening/{screeningId}/booking/{phone}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Kunne ikke slette booking.");
        }
    }

    public Task DeleteBookingAsync(CustomerBookingDto bookingToDelete)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomerBookingDto>> GetBookingsByPhoneAsync(string phone)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.GetAsync($"api/customer/bookings/{phone}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Kunne ikke hente bookinger");

        return await response.Content.ReadFromJsonAsync<List<CustomerBookingDto>>() 
               ?? new List<CustomerBookingDto>();
    }

}