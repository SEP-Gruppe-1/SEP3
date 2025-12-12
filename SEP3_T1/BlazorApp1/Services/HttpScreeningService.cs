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
    public async Task<ScreenDto> GetScreeningAsync(int id)
    {
        await jwtHandler.AttachJwtAsync(http);
        return await http.GetFromJsonAsync<ScreenDto>($"api/screening/{id}")
               ?? throw new Exception("Could not load screening.");
    }

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

    public Task BookSeatsAsync(int screeningId, List<int> seatIds, string phone)
    {
       throw new NotImplementedException();
    }
}