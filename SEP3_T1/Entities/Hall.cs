namespace Entities;

public class Hall
{
    public static readonly Dictionary<int, Hall> instance = new();
    public int Number { get; set; }
    public int LayoutId { get; set; }
    public int Capacity { get; set; }
    public List<Seat> Seats { get; set; } = new();
    public Layout Layout { get; set; }
    public int Id { get; set; }

    private Hall(int id)
    {
        Id = id;
        Number = id;
        

        Seats = new List<Seat>();
     


    }
    
    public void InitializeLayoutAndSeats()
    {
        Layout = Layout.GetInstance(LayoutId);

        if (Layout == null)
            throw new Exception($"Layout {LayoutId} not loaded before Hall {Id}!");

        int rows = Layout.maxLetter - 'A' + 1;
        int seatsPerRow = Layout.maxSeatInt;

        Capacity = rows * seatsPerRow;
        GenerateSeats(rows, seatsPerRow);
    }
    private void GenerateSeats(int rows, int seatsPerRow)
    {
        for (var r = 0; r < rows; r++)
        {
            var rowChar = (char)('A' + r);

            for (var s = 1; s <= seatsPerRow; s++)
                Seats.Add(new Seat
                {
                    Row = rowChar,
                    Number = s,
                    IsBooked = false,
                    Price = 100 // Hvis du vil have denne med
                });
        }
    }


    public static Hall GetInstance(int id)
    {
        if (!instance.ContainsKey(id)) instance[id] = new Hall(id);
        return instance[id];
    }
}