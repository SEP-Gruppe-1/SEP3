using ApiContract;
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
                Name = dtoCustomer.Name,
                Email = dtoCustomer.Email,
                Password = dtoCustomer.Password
            });

        return await Task.FromResult(customers);
    }

    public async Task<DTOCustomer> GetCustomerByPhoneAsync(int phone)
    {
        var response = await _client.GetCustomerByPhoneAsync(new GetCustomerByPhoneRequest { Phone = phone });
        return response.Customer;
    }


    public async Task<Customer> SaveCustomerAsync(Customer customer)
    {
        var dto = new DTOCustomer
        {
            Name = customer.Name,
            Email = customer.Email,
            Password = customer.Password,
            Phone = customer.Phone
        };

        var request = new SaveCustomerRequest { Customer = dto };

        var response = await _client.SaveCustomerAsync(request);

        // map DTO back to your Customer entity
        return new Customer
        {
            Name = response.Customer.Name,
            Email = response.Customer.Email,
            Password = response.Customer.Password,
            Phone = response.Customer.Phone
        };
    }

    public async Task<List<Hall>> GetHallsAsync()
    {
        var response = await _client.GetHallsAsync(new GetHallsRequest());
        Console.WriteLine("gRPC returned hall count: " + response.Halls.Count);

        var halls = new List<Hall>();

        foreach (var dto in response.Halls)
        {
            Console.WriteLine($"Hall from gRPC: {dto.Id}, {dto.Number}, {dto.Layout}");

            halls.Add(new Hall
            {
                Id = dto.Id,
                Number = dto.Number,
                LayoutId = dto.Layout,
                Capacity = 0,
                Seats = new List<Seat>()
            });
        }

        return await Task.FromResult(halls);
        
    }

    public async Task<Hall> GetHallByIdAsync(int id)
    {
        var response = await _client.GetHallByIDAsync(new GetHallByIdRequest { Id = id });
        var dto = response.Hall;

        return new Hall
        {
            Id = dto.Id,
            Number = dto.Number,
            LayoutId = dto.Layout,
            Capacity = 0,
            Seats = new List<Seat>()
        };
    }
}