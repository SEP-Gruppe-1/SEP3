using gRPCRepositories;

namespace gRPCRepositories;

public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new CinemaServiceClient("http://localhost:9090");

        var customers = await client.GetCustomersAsync();
        foreach (var customer in customers)
        {
            Console.WriteLine($"Customer: {customer.Name}");
        }
    }
}