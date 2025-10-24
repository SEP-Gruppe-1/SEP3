

namespace Entities;


public class Hall
{
  public List<Seat> Seats { get; private set; } = new List<Seat>();
    public List<Seat> BookedSeats { get; private set; } = new List<Seat>();
    
    public bool HasSelectedSeats => Seats.Any(s => s.IsSelected);
    public int SelectedSeatsCount => Seats.Count(s => s.IsSelected);
    public decimal TotalPrice => Seats.Where(s => s.IsSelected).Sum(s => s.Price);
    public int TotalBookedSeats => BookedSeats.Count;
    
 
}