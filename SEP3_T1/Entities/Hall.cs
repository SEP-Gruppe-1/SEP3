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


        InitializeLayoutAndSeats();
    }

    public void InitializeLayoutAndSeats()
    {
        Layout = Layout.GetInstance(LayoutId);

        if (Layout == null)
            throw new Exception($"Layout {LayoutId} not loaded before Hall {Id}!");

        int rows = Layout.maxLetter - 'A' + 1;
        int seatsPerRow = Layout.maxSeatInt;

        Capacity = rows * seatsPerRow;
        
    }

    


    public static Hall GetInstance(int id)
    {
        if (!instance.ContainsKey(id)) instance[id] = new Hall(id);
        return instance[id];
    }
}