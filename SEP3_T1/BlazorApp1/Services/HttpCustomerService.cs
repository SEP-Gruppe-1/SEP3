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
    
    public async  Task<List<CustomerDto>> GetCustomers()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/Customer");
        String responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting Customer: {responseContent}");
        }

        return JsonSerializer.Deserialize<List<CustomerDto>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    
    }

    public Task<CustomerDto?> GetByPhoneAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveCustomerAsync(CustomerCreateDto customer)
    {
        throw new NotImplementedException();
    }
}