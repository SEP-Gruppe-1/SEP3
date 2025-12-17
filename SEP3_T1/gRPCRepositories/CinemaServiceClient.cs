using System.Globalization;
using ApiContract;
using Entities;
using Grpc.Core;
using Grpc.Net.Client;
using Grpccinema;
using BookSeatsRequest = Grpccinema.BookSeatsRequest;

namespace gRPCRepositories;

public class CinemaServiceClient
{
    private readonly CinemaService.CinemaServiceClient client;

    public CinemaServiceClient(string serverAddress)
    {
        var channel = GrpcChannel.ForAddress(serverAddress);
        client = new CinemaService.CinemaServiceClient(channel);
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var response = await client.GetCustomersAsync(new GetCustomersRequest());
        var customers = new List<Customer>();
        foreach (var dtoCustomer in response.Customers)
            customers.Add(new Customer
            {
                Phone = dtoCustomer.Phone,
                Name = dtoCustomer.Name,
                Email = dtoCustomer.Email,
                Password = dtoCustomer.Password,
                Role = dtoCustomer.Role
            });

        return await Task.FromResult(customers);
    }

    // Eksempel på RPC error handling 
    public async Task<DTOCustomer> GetCustomerByPhoneAsync(string phone)
    {
        try
        {
            var response = await client.GetCustomerByPhoneAsync(new GetCustomerByPhoneRequest { Phone = phone });
            return response.Customer;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }


    public async Task<Customer> SaveCustomerAsync(Customer customer)
    {
        var dto = new DTOCustomer
        {
            Name = customer.Name,
            Email = customer.Email,
            Password = customer.Password,
            Phone = customer.Phone,
            Role = customer.Role
        };

        var request = new SaveCustomerRequest { Customer = dto };

        var response = await client.SaveCustomerAsync(request);

        
        return new Customer
        {
            Name = response.Customer.Name,
            Email = response.Customer.Email,
            Password = response.Customer.Password,
            Phone = response.Customer.Phone,
            Role = response.Customer.Role
        };
    }
    
    public async Task DeleteCustomerAsync(string phone)
    {
        var request = new DeleteCustomerRequest
        {
            Phone = phone
        };

        await client.DeleteCustomerAsync(request);
    }
    
    public async Task DeleteBookingAsync(int screeningId, string phone)
    {
        var request = new DeleteBookingRequest
        {
            ScreeningId = screeningId,
            PhoneNumber = phone
        };

        await client.DeleteBookingAsync(request);
    }
    
    public async Task<bool> VerifyCustomerPasswordAsync(string phone, string password)
    {
        var request = new VerifyCustomerPasswordRequest
        {
            Phone = phone,
            Password = password
        };
        var response = await client.VerifyCustomerPasswordAsync(request);
        return response.IsValid;
    }


    public async Task<List<Hall>> GetHallsAsync()
    {

        var response = await client.GetHallsAsync(new GetHallsRequest());
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
        var response = await client.GetHallByIDAsync(new GetHallByIdRequest { Id = id });
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
        var response = await client.GetAllScreeningsAsync(new GetAllScreeningsRequest());
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
    
    public async Task<Screening> SaveScreeningAsync(Screening screening)
    {
        var dtoScreening = new DTOScreening
        {
            Id = screening.screeningId,
            Movie = new DTOMovie
            {
                Id = screening.movie.MovieId,
                Title = screening.movie.MovieTitle,
                Genre = screening.movie.Genre,
                Playtime = screening.movie.DurationMinutes,
                ReleaseDate = screening.movie.ReleaseDate
            },
            Hall = new DTOHall
            {
                Id = screening.hall.Id,
                Number = screening.hall.Number,
                Layout = screening.hall.LayoutId
            },
            HallId = screening.hallId,
            StartTime = screening.startTime.ToString("HH:mm", CultureInfo.InvariantCulture),
            Date = screening.date.ToString("yyyy-MM-dd"),
            AvailableSeats = screening.availableSeats
        };

        var request = new SaveScreeningRequest { Screening = dtoScreening };

        var response = await client.SaveScreeningAsync(request);

        var savedDto = response.Screening;

        return new Screening
        {
            screeningId = savedDto.Id,
            movie = ConvertToMovie(savedDto.Movie),
            hall = ConvertToHall(savedDto.Hall),
            hallId = savedDto.HallId,
            startTime = TimeOnly.Parse(savedDto.StartTime),
            date = DateOnly.Parse(savedDto.Date),
            availableSeats = savedDto.AvailableSeats
        };
       
    }

    private Movie ConvertToMovie(DTOMovie dto)
    {
        return new Movie
        {
            MovieId = dto.Id,
            MovieTitle = dto.Title,
            Genre = dto.Genre,
            DurationMinutes = dto.Playtime,
            ReleaseDate = dto.ReleaseDate,
            description = dto.Description,
            poster_Url = dto.PosterUrl,
            banner_Url = dto.BannerUrl
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

    
    private Customer ConvertToCustomer(DTOCustomer dto)
    {
        if (dto == null)
            return null;
        
        return new Customer
        {
            Phone = dto.Phone,
            Name = dto.Name,
            Email = dto.Email
        };
    }
    public async Task<List<Movie>> GetMoviesAsync()
    {
        var response = await client.GetAllMoviesAsync(new GetAllMoviesRequest());
        var movies = new List<Movie>();
        foreach (var dtoMovie in response.Movies)
            movies.Add(new Movie
            {
                MovieId = dtoMovie.Id,
                MovieTitle = dtoMovie.Title,
                DurationMinutes = dtoMovie.Playtime,
                Genre = dtoMovie.Genre,
                ReleaseDate = dtoMovie.ReleaseDate,
                description = dtoMovie.Description,
                poster_Url = dtoMovie.PosterUrl,
                banner_Url = dtoMovie.BannerUrl
            });
        return await Task.FromResult(movies);
    }

    
    public async Task<Movie> SaveMovieAsync(Movie movie)
    {
        var dto = new DTOMovie
        {
            Id = movie.MovieId,                       
            Title = movie.MovieTitle,
            Genre = movie.Genre,
            Playtime = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate, 
            Description = movie.description ?? "",
            PosterUrl = movie.poster_Url ?? "",
            BannerUrl = movie.banner_Url ?? ""
        };

        var request = new SaveMovieRequest { Movie = dto };
        var response = await client.SaveMovieAsync(request).ResponseAsync;


        var saved = response.Movie;

        return new Movie
        {
            MovieId = saved.Id,
            MovieTitle = saved.Title,
            Genre = saved.Genre,
            DurationMinutes = saved.Playtime,
            ReleaseDate = saved.ReleaseDate,
            description = saved.Description,
            poster_Url = saved.PosterUrl,
            banner_Url = saved.BannerUrl
        };
    }


    public async Task<DTOMovie> getMovieById(int id)
    {
        var response = await client.GetMovieByIDAsync(new GetMovieByIdRequest { Id = id });
        return response.Movie;
    }
    
    
    public async Task<List<Layout>> GetLayoutsAsync()
    {
        var response = await client.GetAllLayoutsAsync(new GetAllLayoutsRequest());
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

    public async Task<List<Seat>> GetSeatsAsync()
    {
        var response = await client.GetAllSeatsAsync(new GetAllSeatsRequest());
        var seats = new List<Seat>();
        foreach (var dtoSeat in response.Seats)
        {
            var seat = new Seat
            {
                id = dtoSeat.Id,
                Row = dtoSeat.Letter[0],
                Number = dtoSeat.Number,
                IsBooked = dtoSeat.Booked,
                Customer = ConvertToCustomer(dtoSeat.Customer)
            };
            seats.Add(seat);
        }
        return await Task.FromResult(seats);
    }
   
   public async Task<List<Seat>> GetSeatsByScreeningIdAsync(int screeningId)
    {
        var response = await client.GetSeatsByScreeningAsync(new GetSeatsByScreeningRequest { ScreeningId = screeningId });
        var seats = new List<Seat>();
        foreach (var dtoSeat in response.Seats)
        {
            var seat = new Seat
            {
                id = dtoSeat.Id,
                Row = dtoSeat.Letter[0],
                Number = dtoSeat.Number,
                IsBooked = dtoSeat.Booked,
                Customer = dtoSeat.Customer != null ? ConvertToCustomer(dtoSeat.Customer) : null 
            };
            seats.Add(seat);
        }
        return await Task.FromResult(seats);
    }
   
    public async Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber)
    {
        var request = new BookSeatsRequest
        {
            ScreeningId = screeningId,
            CustomerPhone = phoneNumber
        };

        request.SeatIds.AddRange(seatIds);

        await client.BookSeatsAsync(request);
    }
    public async Task<UpdateBookingResponse> UpdateBookingAsync(int screeningId, string phoneNumber, List<int> seatsToAdd, List<int> seatsToRemove)
    {
        var request = new UpdateBookingRequest
        {
            ScreeningId = screeningId,
            CustomerPhone = phoneNumber
        };

        request.SeatsToAdd.AddRange(seatsToAdd);
        request.SeatsToRemove.AddRange(seatsToRemove);

        var response = await client.UpdateBookingAsync(request);
        return response;
    }

   
}