using Entities;

namespace RepositoryContracts;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(string phone, string name, string email);
    Task<Customer?> GetSingleAsync(String phone);
    IQueryable<Customer> GetAll();
    Task VerifyCustomerDoesNotExist(string phone, string email);

    Task SaveCustomer(Customer customer);
}