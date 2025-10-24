
using Entities;
using RepositoryContract;


namespace InMemoryRepositories;
public class HallInMemoryRepository : IHallRepository
{
    private  Hall Hall { get; } = new();

      public void Initialize(int rows, int seatsPerRow)
    {
        Hall.Seats.Clear();
        Hall.BookedSeats.Clear();
        
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            char rowLetter = (char)('A' + rowIndex);
            
            for (int seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
            {
                Hall.Seats.Add(new Seat
                {
                    Row = rowLetter,
                    Number = seatNumber,
                    Price = 100m
                });
            }
        }
        

    }

    public void ToggleSeatSelection(Seat seat)
    {
        // Kun tillad selection hvis sædet ikke er booket
        if (!seat.IsBooked)
        {
            var targetSeat = Hall.Seats.FirstOrDefault(s => s.Row == seat.Row && s.Number == seat.Number);
            if (targetSeat != null)
            {
                targetSeat.IsSelected = !targetSeat.IsSelected;
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var seat in Hall.Seats)
        {
            seat.IsSelected = false;
        }
    }

    public void BookSelectedSeats()
    {
        var selectedSeats = GetSelectedSeats();
        foreach (var seat in selectedSeats)
        {
            seat.IsBooked = true;
            seat.IsSelected = false; // Fjern selection efter booking
            Hall.BookedSeats.Add(seat);
        }
    }

    public List<Seat> GetSelectedSeats()
    {
        return Hall.Seats.Where(s => s.IsSelected)
                   .OrderBy(s => s.Row)
                   .ThenBy(s => s.Number)
                   .ToList();
    }

    public List<Seat> GetAvailableSeats()
    {
        return Hall.Seats.Where(s => !s.IsBooked)
                   .OrderBy(s => s.Row)
                   .ThenBy(s => s.Number)
                   .ToList();
    }

    public string GetSelectedSeatsDisplay()
    {
        return string.Join(", ", Hall.Seats
            .Where(s => s.IsSelected)
            .OrderBy(s => s.Row)
            .ThenBy(s => s.Number)
            .Select(s => $"{s.Row}{s.Number}"));
    }

    public string GetBookedSeatsDisplay()
    {
        return string.Join(", ", Hall.BookedSeats.OrderBy(s => s.Row)
                                           .ThenBy(s => s.Number)
                                           .Select(s => $"{s.Row}{s.Number}"));
    }

    public Seat GetSeat(char row, int number)
    {
        return Hall.Seats.FirstOrDefault(s => s.Row == row && s.Number == number);
    }

    public Hall getHall()
    {
        return Hall;
    }
}