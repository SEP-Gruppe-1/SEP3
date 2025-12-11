using Entities;

namespace ApiContract;

public record ScreenDto(
    int ScreeningId,
    Movie Movie,
    Hall hall,
    int hallId,
    List<Seat> seats,
    TimeOnly StartTime,
    DateOnly Date,
    int AvailableSeats);
    
public record BookSeatsRequest(
    List<int> SeatIds,
    string PhoneNumber);
    
public record UpdateBookingDTO(

    List<int> SeatsToAdd,
    List<int> SeatsToRemove,
    string CustomerPhone);