using System.Net;
using ApiContract;

namespace BlazorApp1.Services;

/// <summary>
/// Interface for Customer Service
/// </summary>

public interface ICustomerService
{
    Task<List<CustomerDto>> GetCustomers();
    Task<CustomerDto?> GetByPhoneAsync();
    Task SaveCustomerAsync(SaveCustomerDto customer);
    Task UpdateCustomerRoleAsync(string phone, string newRole);
    Task DeleteCustomerAsync(string phone);
    Task<List<CustomerBookingDto>> GetMyBookingsAsync(string phone);


    Task<CustomerDto?> GetSingleCustomerAsync(string phone);
}