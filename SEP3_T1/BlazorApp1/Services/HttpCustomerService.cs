using System.Net;
using System.Text.Json;
using ApiContract;

namespace BlazorApp1.Services;

public class HttpCustomerService : ICustomerService
{
    public readonly HttpClient _httpClient;

    public HttpCustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CustomerDto>> GetCustomers()
    {
        var response = await _httpClient.GetAsync("api/Customer");
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new Exception($"Error getting Customer: {responseContent}");

        return JsonSerializer.Deserialize<List<CustomerDto>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public Task<CustomerDto?> GetByPhoneAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SaveCustomerAsync(SaveCustomerDto customer)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Customer", customer);

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var errorObj = await response.Content.ReadFromJsonAsync<ConflictResponse>();
            var msg = errorObj?.Message ?? "Telefonnummer eller email er allerede registreret.";
            throw new Exception(msg);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error adding Customer: {responseContent}");
    }

    public class ConflictResponse
    {
        public string? Message { get; set; }
    }
}