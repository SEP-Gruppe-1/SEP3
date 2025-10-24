using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class SeatInMemoryRepository : ISeatRepository
{
    private readonly Seat Seats = new();

    public string ToString()

    {
        return $"{Seats.Row}{Seats.Number} ({(Seats.IsBooked ? "Booket" : Seats.IsSelected ? "Valgt" : "Ledig")})";
    }
    
}