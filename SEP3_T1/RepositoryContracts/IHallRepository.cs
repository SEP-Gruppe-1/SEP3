

public interface IHallRepository
{
   
    public void Initialize(int rows, int seatsPerRow){}
    
    public void ToggleSeatSelection(Seat seat){}
    
    public void ClearSelection(){}
    
    public void BookSelectedSeats(){}

    List<Seat> GetSelectedSeats();
    
    
    public List<Seat> GetAvailableSeats();

    public string GetBookedSeatsDisplay();

    public Seat GetSeat(char row, int number);
}