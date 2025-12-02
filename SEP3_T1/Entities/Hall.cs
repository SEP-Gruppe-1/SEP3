namespace Entities;

public class Hall
{
   public int Number { get; set; }
   public int LayoutId { get; set; }
   public int Capacity { get; set; }
   public List<Seat> Seats { get; set; } = new();
    public int Id { get; set; }
    
    public static readonly Dictionary<int, Hall> instance = new ();

   
    
    private Hall(int id)
    {
        this.Id = id;
        this.Number = id;
        this.LayoutId = id;  // samme som Java: layout == id

        Seats = new List<Seat>();

        switch (LayoutId)
        {
            case 2: // BIG_HALL
                GenerateSeats(8, 12);
                Capacity = 96;
                break;

            case 1: // SMALL_HALL
                GenerateSeats(4, 10);
                Capacity = 40;
                break;

            default:
                throw new ArgumentException("Invalid layout value " + LayoutId);
        }
    }

   

    private void GenerateSeats(int rows, int seatsPerRow)
    {
        for (int r = 0; r < rows; r++)
        {
            char rowChar = (char)('A' + r);

            for (int s = 1; s <= seatsPerRow; s++)
            {
                Seats.Add(new Seat
                {
                    Row = rowChar,
                    Number = s,
                    IsBooked = false,
                    Price = 100   // Hvis du vil have denne med
                });
            }
        }
    }
 
    
    public static Hall GetInstance(int id)
    {
        if (!instance.ContainsKey(id))
        {
            instance[id] = new Hall(id);
        }
        return instance[id];
    }
    
}