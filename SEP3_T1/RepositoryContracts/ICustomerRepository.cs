using Entities;

namespace RepositoryContracts;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(string phone);
    Task<Customer?> GetSingleAsync(string phone);
    IQueryable<Customer> GetAll();
    Task VerifyCustomerDoesNotExist(string phone, string email);

    Task SaveCustomer(Customer customer);
    Task<Customer?> GetByPhoneAndPasswordAsync(string phone, string password);
}