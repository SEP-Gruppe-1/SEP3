using Entities;
using RepositoryContracts;

namespace BlazorApp1.InMemoryRepositories;

public class CustomerInMemoryRepository : ICustomerRepository
{
    private readonly List<Customer> customers = new();

    public Task<Customer> AddAsync(Customer customer)
    {
        customer.Phone = customers.Any()
            ? customers.Max(c => c.Phone) + 1
            : 1;
        customers.Add(customer);
        return Task.FromResult(customer);
    }

    public Task UpdateAsync(Customer customer)
    {
        var existingCustomer = customers.SingleOrDefault(c => c.Phone == customer.Phone);
        if (existingCustomer == null)
            throw new InvalidOperationException($"Customer with phone nr: {customer.Phone} not found.");
        customers.Remove(existingCustomer); 
        customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int phone)
    {
        var customerToRemove = customers.SingleOrDefault(c => c.Phone == phone);    
        if (customerToRemove == null)
            throw new InvalidOperationException($"Customer with phone nr: ,{phone} not found.");
        customers.Remove(customerToRemove);
        return Task.CompletedTask;
    }

    public Task<Customer?> GetSingleAsync(int Phone)
    {
        var customer = customers.SingleOrDefault(c => c.Phone == Phone);
        if (customer == null)
            throw new InvalidOperationException($"Customer with phone nr: {Phone} not found.");
        return Task.FromResult(customer);
    }
    
    public IQueryable<Customer> GetAll()
    {
        return customers.AsQueryable();
    }
}