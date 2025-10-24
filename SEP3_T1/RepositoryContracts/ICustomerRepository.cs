using Entities;

namespace RepositoryContracts;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int phone);
    Task<Customer> GetSingleAsync(int phone);
    Customer getCustomer();
    IQueryable<Customer> GetAll();
}