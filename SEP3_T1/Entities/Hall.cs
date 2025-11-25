namespace Entities;

public class Hall
{
   public int Number { get; set; }
   public int LayoutId { get; set; }
   public int Capacity { get; set; }
   public List<Seat> Seats { get; set; } = new();
    public int Id { get; set; }
 
}