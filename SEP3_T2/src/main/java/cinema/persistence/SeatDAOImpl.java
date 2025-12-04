package cinema.persistence;

import cinema.model.Seat;

import java.sql.*;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class SeatDAOImpl implements SeatDAO {

    public static SeatDAOImpl instance;
    public static ScreeningDAO screeningDAO;

    public SeatDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
        this.screeningDAO= new ScreeningDAOImpl();
    }

    public static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
                "postgres", "123");
    }

    public static SeatDAOImpl getInstance() throws SQLException {
        if (instance == null) {
            instance = new SeatDAOImpl();
        }
        return instance;
    }


    @Override
    public List<Seat> getSeatsByScreening(int screeningId) throws SQLException {
        List<Seat> seats = new ArrayList<>();
        Set<Integer> bookedSeatIds = new HashSet<>();

        // 1. Hent alle bookede seat_id
        String bookedSql =
                "SELECT bs.seat_id " +
                        "FROM Booking b " +
                        "JOIN BookingSeat bs ON b.booking_id = bs.booking_id " +
                        "WHERE b.screening_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(bookedSql)) {
            stmt.setInt(1, screeningId);
            try (ResultSet rs = stmt.executeQuery()) {
                while (rs.next()) {
                    bookedSeatIds.add(rs.getInt("seat_id"));
                }
            }
        }

        // 2. Hent alle seats i hall'en for denne screening
        String seatSql =
                "SELECT s.seat_id, s.row_letter, s.seat_number " +
                        "FROM seat s " +
                        "JOIN Screening sc ON sc.hall_id = s.hall_id " +
                        "WHERE sc.screening_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(seatSql)) {
            stmt.setInt(1, screeningId);
            try (ResultSet rs = stmt.executeQuery()) {
                while (rs.next()) {
                    int id = rs.getInt("seat_id");
                    char row = rs.getString("row_letter").charAt(0);
                    int number = rs.getInt("seat_number");

                    Seat seat = new Seat(row, number, id);

                    if (bookedSeatIds.contains(id)) {
                        seat.bookSeat();
                    }

                    seats.add(seat);
                }
            }
        }

        return seats;
    }

    @Override
    public List<Seat> getAllSeats() {
        return null;
    }

    @Override
    public List<Seat> getBookedSeatsByScreeningId(int screeningId) throws SQLException {
        List<Seat> bookedSeats = new ArrayList<>();
        String sql =
                "SELECT s.seat_id \n" +
                        "FROM Booking b \n" +
                        "JOIN BookingSeat bs ON b.booking_id = bs.booking_id \n" +
                        "JOIN seat s ON s.seat_id = bs.seat_id \n" +
                        "WHERE b.screening_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, screeningId);
            try (ResultSet rs = stmt.executeQuery()) {

                while (rs.next()) {
                    int seatId = rs.getInt("seat_id");
                    bookedSeats.get(seatId).bookSeat();
                }
            }
            }


        return bookedSeats;
    }
}
