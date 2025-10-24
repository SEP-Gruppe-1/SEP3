using System;
using System.Collections.Generic;

namespace Entities;



public class Booking
{
    public string BookingId { get; set; } = "";
    public List<Seat> Seats { get; set; } = new List<Seat>();
    public decimal TotalPrice { get; set; }
    public DateTime BookingTime { get; set; }

    

}