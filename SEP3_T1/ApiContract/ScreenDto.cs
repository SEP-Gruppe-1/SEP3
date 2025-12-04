using Entities;

namespace ApiContract;

public record ScreenDto(
    int ScreeningId,
    Movie Movie,
    Hall hall,
    int hallId,
    TimeOnly StartTime,
    DateOnly Date,
    int AvailableSeats);