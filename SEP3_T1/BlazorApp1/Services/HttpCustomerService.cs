using System.Net;
using System.Text.Json;
using ApiContract;

namespace BlazorApp1.Services;

/// <summary>
/// Http implementation of ICustomerService
/// </summary>

public class HttpCustomerService : ICustomerService
{
    public readonly HttpClient httpClient;
    private readonly JwtHttpClientHandler jwtHandler;
    
    /// <summary>
    /// HttpCustomerService constructor
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="jwtHandler"></param>

    public HttpCustomerService(HttpClient httpClient, JwtHttpClientHandler jwtHandler )
    {
        this.httpClient = httpClient;
        this.jwtHandler = jwtHandler;
    }
    
    /// <summary>
    /// get all customers
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public async Task<List<CustomerDto>> GetCustomers()
    {
        await jwtHandler.AttachJwtAsync(httpClient);
        var response = await httpClient.GetAsync("api/Customer");
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
    /// <summary>
    /// Save a new customer
    /// </summary>
    /// <param name="customer"></param>
    /// <exception cref="Exception"></exception>

    public async Task SaveCustomerAsync(SaveCustomerDto customer)
    {
        await jwtHandler.AttachJwtAsync(httpClient);
        var response = await httpClient.PostAsJsonAsync("api/Customer", customer);

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
    /// <summary>
    /// Conflict response model
    /// </summary>

    public class ConflictResponse
    {
        public string? Message { get; set; }
    }
    
    /// <summary>
    /// Update customer role
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="newRole"></param>
    /// <exception cref="Exception"></exception>
    
    public async Task UpdateCustomerRoleAsync(string phone, string newRole)
    {
        await jwtHandler.AttachJwtAsync(httpClient);

        var response = await httpClient.PutAsJsonAsync(
            "api/customer/role",
            new
            {
                Phone = phone,
                NewRole = newRole
            });

        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }
    
    /// <summary>
    /// Delete customer by phone
    /// </summary>
    /// <param name="phone"></param>
    /// <exception cref="Exception"></exception>

    public async Task DeleteCustomerAsync(string phone)
    {
        await jwtHandler.AttachJwtAsync(httpClient); 

        var response = await httpClient.DeleteAsync($"api/customer/{phone}");

        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }


}