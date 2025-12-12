namespace ApiContract;

public record SeatDto(
    int Id,
    int Row,
    int Number,
    bool IsBooked,
    string? CustomerPhone
);