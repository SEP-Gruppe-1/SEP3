using Entities;

namespace RepositoryContracts;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int phone, string name, string email);
    Task<Customer?> GetSingleAsync(int phone, string name, string email);
    IQueryable<Customer> GetAll();
}