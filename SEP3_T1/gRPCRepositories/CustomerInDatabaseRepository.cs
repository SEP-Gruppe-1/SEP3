using Entities;
using RepositoryContracts;

namespace gRPCRepositories;

public class CustomerInDatabaseRepository : ICustomerRepository
{
    private readonly CinemaServiceClient client;

    private List<Customer> customers;

    public CustomerInDatabaseRepository()
    {
        client = new CinemaServiceClient("http://localhost:9090");
        customers = new List<Customer>();
    }

    public async Task InitializeAsync()
    {
        customers = await client.GetCustomersAsync();
    }

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
        var existingCustomer = customers.SingleOrDefault(c => c.Phone == c.Phone);
        if (existingCustomer == null)
            throw new InvalidOperationException($"Customer with phone nr: {customer.Phone} not found.");
        customers.Remove(existingCustomer);
        customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int phone, string name, string email)
    {
        throw new NotImplementedException();
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