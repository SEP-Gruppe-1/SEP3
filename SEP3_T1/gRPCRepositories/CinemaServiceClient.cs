using Entities;
using Grpc.Net.Client;
using Grpccinema;

namespace gRPCRepositories;

public class CinemaServiceClient
{
    private readonly CinemaService.CinemaServiceClient _client;

    public CinemaServiceClient(string serverAddress)
    {
        var channel = GrpcChannel.ForAddress(serverAddress);
        _client = new CinemaService.CinemaServiceClient(channel);
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var response = await _client.GetCustomersAsync(new GetCustomersRequest());
        var customers = new List<Customer>();
        foreach (var dtoCustomer in response.Customers)
            customers.Add(new Customer
            {
                Phone = dtoCustomer.Phone,
                Name = dtoCustomer.Name
                // Email = dtoCustomer.Email
            });

        return await Task.FromResult(customers);
    }

    public async Task<DTOCustomer> GetCustomerByPhoneAsync(int phone)
    {
        var response = await _client.GetCustomerByPhoneAsync(new GetCustomerByPhoneRequest { Phone = phone });
        return response.Customer;
    }
}