using Entities;

namespace ApiContract;

public record ScreenDto(
    int ScreeningId,
    MovieDto Movie,
    HallDto Hall,
    int HallId,
    List<SeatDto> Seats,
    TimeOnly StartTime,
    DateOnly Date,
    int AvailableSeats
);


    
public record BookSeatsRequest(
    List<int> SeatIds,
    string PhoneNumber);
    
public record UpdateBookingDTO(

    List<int> SeatsToAdd,
    List<int> SeatsToRemove,
    string CustomerPhone);
    
    public record ScreeningCreateDto(
       int MovieId,
       int HallId,
       string StartTime,
       string Date);
        
        public record ScreeningDto(int ScreeningId,
            Movie Movie,
            Hall hall,
            int hallId,
            List<Seat> seats,
            TimeOnly StartTime,
            DateOnly Date,
            int AvailableSeats);