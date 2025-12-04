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
                Email = dtoCustomer.Email
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

            var hall = Hall.GetInstance(dto.Id);
            hall.Number = dto.Number;
            hall.LayoutId = dto.Layout;
            hall.Id = dto.Id;
        }

        return await Task.FromResult(halls);
    }

    public async Task<Hall> GetHallByIdAsync(int id)
    {
       
        
        var response = await _client.GetHallByIDAsync(new GetHallByIdRequest { Id = id });
        var dto = response.Hall;

     
        var hall = Hall.GetInstance(id);
        
       
        
        hall.Number = dto.Number;
        hall.LayoutId = dto.Layout;
        hall.Id = dto.Id;

        return hall;
    }

    public async Task<List<Screening>> GetScreeningsAsync()
    {
        
        await GetLayoutsAsync();
        var response = await _client.GetAllScreeningsAsync(new GetAllScreeningsRequest());
        var screenings = new List<Screening>();

        foreach (var dtoScreening in response.Screenings)
        {
            var dtoMovie = await getMovieById(dtoScreening.Movie.Id);
            var movie = ConvertToMovie(dtoMovie);
           
            var timeOnly = TimeOnly.Parse(dtoScreening.StartTime);
            var dateOnly = DateOnly.Parse(dtoScreening.Date);
            screenings.Add(new Screening
            {
                movie = movie,
                screeningId = dtoScreening.Id,
                hall = ConvertToHall(dtoScreening.Hall),
                hallId = dtoScreening.HallId,
                startTime = timeOnly,
                date = dateOnly,
                availableSeats = dtoScreening.AvailableSeats
            });
        }

        return await Task.FromResult(screenings);
    }

    private Movie ConvertToMovie(DTOMovie dto)
    {
        return new Movie
        {
            MovieId = dto.Id,
            MovieTitle = dto.Title,
            Genre = dto.Genre,
            DurationMinutes = dto.Playtime,
            ReleaseDate = dto.ReleaseDate
        };
    }
    
    private Hall ConvertToHall(DTOHall dto)
    {
        var hall = Hall.GetInstance(dto.Id);
        hall.Number = dto.Number;
        hall.LayoutId = dto.Layout;
        hall.Id = dto.Id;
        return hall;
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var response = await _client.GetAllMoviesAsync(new GetAllMoviesRequest());
        var movies = new List<Movie>();
        foreach (var dtoMovie in response.Movies)
            movies.Add(new Movie
            {
                MovieId = dtoMovie.Id,
                MovieTitle = dtoMovie.Title,
                DurationMinutes = dtoMovie.Playtime,
                Genre = dtoMovie.Genre,
                ReleaseDate = dtoMovie.ReleaseDate
            });
        return await Task.FromResult(movies);
    }

    public async Task<DTOMovie> getMovieById(int id)
    {
        var response = await _client.GetMovieByIDAsync(new GetMovieByIdRequest { Id = id });
        return response.Movie;
    }
    
    
    public async Task<List<Layout>> GetLayoutsAsync()
    {
        var response = await _client.GetAllLayoutsAsync(new GetAllLayoutsRequest());
        var layouts = new List<Layout>();
        foreach (var dtoLayout in response.Layouts)
        {
            var layout = Layout.Create(dtoLayout.Id, dtoLayout.MaxLetter[0], dtoLayout.MaxSeatInt);
         
         
            layout.maxLetter = dtoLayout.MaxLetter[0];
            layout.maxSeatInt = dtoLayout.MaxSeatInt;
            layout.id= dtoLayout.Id;
            layouts.Add(layout);
        }
        return await Task.FromResult(layouts);
    }
}