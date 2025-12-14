package cinema.persistence;

import cinema.model.Seat;

import java.sql.SQLException;
import java.util.List;

public interface SeatDAO {

    List<Seat> getSeatsByScreening(int screeningId) throws SQLException;
    List<Seat> getAllSeats();
    void bookSeat(int screeningId, String Phone, List<Integer> seatIds) throws SQLException;
    void updateBooking(int screeningId, String phone, List<Integer> seatsToAdd, List<Integer> seatsToRemove) throws SQLException;
    void DeleteBooking(int screeningId , String phone) throws SQLException;
}
