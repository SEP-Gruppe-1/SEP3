namespace ApiContract;

public record ScreenDto(int ScreeningId, MovieDto Movie, int hallId, TimeOnly StartTime, DateOnly Date, int AvailableSeats);