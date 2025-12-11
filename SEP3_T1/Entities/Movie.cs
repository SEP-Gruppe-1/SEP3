namespace Entities;

public class Movie
{
    public string MovieTitle { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public int MovieId { get; set; }
    public string ReleaseDate { get; set; }

    public byte[]? PosterPath { get; set; }

    public byte[]? BannerPath { get; set; }
    public string? Description { get; set; }
}