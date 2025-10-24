
using Entities;
using RepositoryContract;
using RepositoryContracts;

namespace InMemoryRepositories;

public class BookingInMemoryRepository : IBookingRepository
{
    public string BookingId { get; set; } = "";
    public List<Seat> Seats { get; set; } = new List<Seat>();
    public decimal TotalPrice { get; set; }
    public DateTime BookingTime { get; set; }
    public string toString()
    {
  
    
        var seatIds = string.Join(", ", Seats.Select(s => s.Id));
        return $"Booking {BookingId}: {seatIds} - {TotalPrice} kr - {BookingTime:yyyy-MM-dd HH:mm}";
    
    }
}