using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities;
    
public class Booking
{
    public string BookingId { get; set; } = "";
    public List<Seat> Seats { get; set; } = new List<Seat>();
    public decimal TotalPrice { get; set; }
    public DateTime BookingTime { get; set; }

    
    public override string ToString()
    {
        var seatIds = string.Join(", ", Seats.Select(s => s.Id));
        return $"Booking {BookingId}: {seatIds} - {TotalPrice} kr - {BookingTime:yyyy-MM-dd HH:mm}";
    }
}