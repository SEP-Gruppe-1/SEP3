namespace gRPCRepositories;

using Grpccinema;
using Grpc.Net.Client;

public class CinemaServiceClient
{
    private readonly CinemaService.CinemaServiceClient _client;

    public CinemaServiceClient(string serverAddress)
    {
        var channel = GrpcChannel.ForAddress(serverAddress);
        _client = new CinemaService.CinemaServiceClient(channel);
    }

    public async Task<List<DTOCustomer>> GetCustomersAsync()
    {
        var response = await _client.GetCustomersAsync(new GetCustomersRequest());
        return response.Customers.ToList();
    }

    public async Task<DTOCustomer> GetCustomerByPhoneAsync(int phone)
    {
        var response = await _client.GetCustomerByPhoneAsync(new GetCustomerByPhoneRequest { Phone = phone });
        return response.Customer;
    }
}
