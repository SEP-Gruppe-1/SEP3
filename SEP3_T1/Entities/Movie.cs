namespace Entities;

public class Movie
{
    public string MovieTitle { get; set; }
    public string Genre { get; set; }
    public int DurationMinutes { get; set; }
    public int MovieId { get; set; }
    public string ReleaseDate { get; set; }

    public string PosterPath { get; set; }

    public string? Description { get; set; }
}