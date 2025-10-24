

public class HallInMemoryRepository : IHallRepository
{
    private List<Seat> Seats = new();
    private List<Seat> BookedSeats = new();
  

      public void Initialize(int rows, int seatsPerRow)
    {
        Seats.Clear();
        BookedSeats.Clear();
        
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            char rowLetter = (char)('A' + rowIndex);
            
            for (int seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
            {
                Seats.Add(new Seat
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
            var targetSeat = Seats.FirstOrDefault(s => s.Row == seat.Row && s.Number == seat.Number);
            if (targetSeat != null)
            {
                targetSeat.IsSelected = !targetSeat.IsSelected;
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var seat in Seats)
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
            BookedSeats.Add(seat);
        }
    }

    public List<Seat> GetSelectedSeats()
    {
        return Seats.Where(s => s.IsSelected)
                   .OrderBy(s => s.Row)
                   .ThenBy(s => s.Number)
                   .ToList();
    }

    public List<Seat> GetAvailableSeats()
    {
        return Seats.Where(s => !s.IsBooked)
                   .OrderBy(s => s.Row)
                   .ThenBy(s => s.Number)
                   .ToList();
    }

    public string GetSelectedSeatsDisplay()
    {
        var selected = GetSelectedSeats();
        return string.Join(", ", selected.Select(s => $"{s.Row}{s.Number}"));
    }

    public string GetBookedSeatsDisplay()
    {
        return string.Join(", ", BookedSeats.OrderBy(s => s.Row)
                                           .ThenBy(s => s.Number)
                                           .Select(s => $"{s.Row}{s.Number}"));
    }

    public Seat GetSeat(char row, int number)
    {
        return Seats.FirstOrDefault(s => s.Row == row && s.Number == number);
    }
    
}