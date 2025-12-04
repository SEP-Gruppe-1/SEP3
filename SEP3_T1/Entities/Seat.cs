namespace Entities;

public class Seat
{
    public char Row { get; set; }
    public int Number { get; set; }
    public bool IsSelected { get; set; }
    public bool IsBooked { get; set; } // Ny property for bookede sæder
    public decimal Price { get; set; } = 100m;

    public int id {get ; set; }

    public override string ToString()
    {
        return $"{Row}{Number} ({(IsBooked ? "Booket" : IsSelected ? "Valgt" : "Ledig")})";
    }
}