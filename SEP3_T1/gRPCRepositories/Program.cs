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
         var halle = await client.GetHallByIdAsync(1);
         Console.WriteLine($"Fetched hall by ID 1: {halle.Id} with number {halle.Layout.maxLetter}");

        // var screenings = await client.GetScreeningsAsync();
        // foreach (var screening in screenings)
        //     Console.WriteLine($"Screening: {screening.movie} at {screening.startTime} in hall {screening.hall.Id}");
        //
        // var movies = await client.GetMoviesAsync();
        // foreach (var movie in movies)
        //     Console.WriteLine($"Movie title: {movie.MovieTitle} release date {movie.ReleaseDate}");
        //
        // var movieById = await client.getMovieById(1);
        // Console.WriteLine($"Fetched movie by ID 1: {movieById.Id} hall ");
        
      
    }
}