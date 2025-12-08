using Entities;
using RepositoryContracts;

namespace gRPCRepositories;

public class CustomerInDatabaseRepository : ICustomerRepository
{
    private readonly CinemaServiceClient client;

    private List<Customer> customers;

    public CustomerInDatabaseRepository(CinemaServiceClient client)
    {
        this.client = client;
        customers = new List<Customer>();
    }

    public Task<Customer> AddAsync(Customer customer)
    {
        customer.Phone = customers.Any()
            ? customers.Max(c => c.Phone) + 1
            : "1";
        customers.Add(customer);
        return Task.FromResult(customer);
    }

    public Task UpdateAsync(Customer customer) // Tjek hele metoden, det må kunne laves smartere end at fjerne hele customer og tilføje igen
    {
        var existingCustomer = customers.SingleOrDefault(c => c.Phone == customer.Phone);
        if (existingCustomer == null)
            throw new InvalidOperationException($"Customer with phone nr: {customer.Phone} not found.");
        customers.Remove(existingCustomer);
        customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string phone, string name, string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> GetSingleAsync(string Phone)
    {
        var customers = await GetCustomersAsync();
        return customers.SingleOrDefault(c => c.Phone == Phone)
               ?? throw new InvalidOperationException($"Customer with phone nr: {Phone} not found.");
    }

    public IQueryable<Customer> GetAll()
    {
        return customers.AsQueryable();
    }

    public async Task VerifyCustomerDoesNotExist(string phone, string email)
    {
        var customers = await GetCustomersAsync();
        var CustomerPhoneExists = customers.Any(c => c.Phone == phone);
        if (CustomerPhoneExists) throw new InvalidOperationException($"Phone number: {phone} already exists.");
    }

    public async Task SaveCustomer(Customer customer)
    {
        var savedCustomer = await client.SaveCustomerAsync(customer);


        var existing = customers.SingleOrDefault(c => c.Phone == savedCustomer.Phone);
        if (existing != null) customers.Remove(existing);

        customers.Add(savedCustomer);
    }

    public async Task InitializeAsync()
    {
        customers = await client.GetCustomersAsync();
    }

    public Task DeleteAsync(string phone)
    {
        var customerToRemove = customers.SingleOrDefault(c => c.Phone == phone);
        if (customerToRemove == null)
            throw new InvalidOperationException($"Customer with phone nr: ,{phone} not found.");
        customers.Remove(customerToRemove);
        return Task.CompletedTask;
    }


    private async Task<List<Customer>> GetCustomersAsync()
    {
        var customerAsJson = await client.GetCustomersAsync();
        return customerAsJson;
    }

    public Task<Customer?> GetByPhoneAndPasswordAsync(string phone, string password)
    {
        var customer = customers.SingleOrDefault(c => c.Phone == phone && c.Password == password);

        return Task.FromResult(customer);
    }
}