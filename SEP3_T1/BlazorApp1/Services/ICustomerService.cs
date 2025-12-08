using System.Net;
using ApiContract;

namespace BlazorApp1.Services;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetCustomers();
    Task<CustomerDto?> GetByPhoneAsync();
    Task SaveCustomerAsync(SaveCustomerDto customer);
    Task UpdateCustomerRoleAsync(string phone, string newRole);

    
    
    
}