namespace gRPCRepositories;

public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new CinemaServiceClient("http://localhost:9090");

        var customers = await client.GetCustomersAsync();
        foreach (var customer in customers) Console.WriteLine($"Customer: {customer.Name}");

        // var halls = await client.GetHallsAsync();
        // foreach (var hall in halls) Console.WriteLine($"Hall: {hall.Id} with number {hall.Number}");
        //
        // var halle = await client.GetHallByIdAsync(1);
        // Console.WriteLine($"Fetched hall by ID 1: {halle.Id} with number {halle.Number}");

        var screenings = await client.GetScreeningsAsync();
        foreach (var screening in screenings)
            Console.WriteLine($"Screening: {screening.movie} at {screening.startTime}");

        var movies = await client.GetMoviesAsync();
        foreach (var movie in movies)
            Console.WriteLine($"Movie title: {movie.MovieTitle} release date {movie.ReleaseDate}");

        var movieById = await client.getMovieById(1);
        Console.WriteLine($"Fetched movie by ID 1: {movieById.Id}");

        var customerByPhone = await client.GetCustomerByPhoneAsync("5550001");
        Console.WriteLine($"Fetched customer by phone 5550001: {customerByPhone.Name}");
        
        var seats = await client.GetSeatsByScreeningIdAsync(1);
        foreach (var seat in seats)
            Console.WriteLine($"Seat: Row {seat.Row} Number {seat.Number} Booked: {seat.IsBooked}");
        
    }
}