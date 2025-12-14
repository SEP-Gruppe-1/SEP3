namespace ApiContract;

public record MovieDto(int DurationMinutes, int MovieId, string ReleaseDate, string MovieTitle, string Genre, string? description, string poster_Url, string banner_Url);