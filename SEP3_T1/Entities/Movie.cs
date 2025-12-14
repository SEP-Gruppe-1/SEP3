namespace Entities;

public class Movie
{
    public string MovieTitle { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public int MovieId { get; set; }
    public string ReleaseDate { get; set; }
    
    public string poster_Url { get; set; } = null!;
    
    public string banner_Url { get; set; } = null!;
    public string? description { get; set; }
}