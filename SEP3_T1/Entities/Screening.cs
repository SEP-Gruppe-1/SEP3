namespace Entities;

/// <summary>
/// Screening entity
/// Stores information about a movie screening, including the movie being shown,
/// the hall where it takes place, available seats, and timing details.
/// </summary>

public class Screening
{
    public Movie movie { get; set; }

    public Hall hall { get; set; }
    public int hallId { get; set; }
    public int availableSeats { get; set; }
    public int screeningId { get; set; }

    public TimeOnly endTime { get; set; }
    public TimeOnly startTime { get; set; }
    public DateOnly date { get; set; }
}