package cinema.persistence;

import cinema.model.Customer;
import cinema.model.Seat;

import java.sql.*;
import java.util.*;

public class SeatDAOImpl implements SeatDAO {

    public static SeatDAOImpl instance;


    private SeatDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());


    }

    private static Connection getConnection() throws SQLException {
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
        Map<Integer, String> bookedSeatCustomer = new HashMap<>();

        // 1. Hent alle bookede seat_id
        String bookedSql =
                "SELECT bs.seat_id, b.customer_phone  " +
                        "FROM Booking b " +
                        "JOIN BookingSeat bs ON b.booking_id = bs.booking_id " +
                        "WHERE b.screening_id = ?";

        try (Connection conn = getConnection();


             PreparedStatement stmt = conn.prepareStatement(bookedSql)) {
            stmt.setInt(1, screeningId);
            try (ResultSet rs = stmt.executeQuery()) {
                while (rs.next()) {
                    int seatId = rs.getInt("seat_id");
                    String customerPhone = rs.getString("customer_phone");
                    bookedSeatCustomer.put(seatId, customerPhone);

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
                    CustomerDAO customerDAO1 = CustomerDAOImpl.getInstance();


                    if (bookedSeatCustomer.containsKey(id)) {
                        String customerPhone = bookedSeatCustomer.get(id);


                        Customer customer = customerDAO1.getCustomerByPhone(customerPhone);
                        seat.bookSeat(customer);
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
    public void bookSeat(int screeningId, String phone, List<Integer> seatIds) throws SQLException {
        String checkSeatsSQL =
                "SELECT bs.seat_id " +
                        "FROM BookingSeat bs " +
                        "JOIN Booking b ON bs.booking_id = b.booking_id " +
                        "WHERE b.screening_id = ? AND bs.seat_id = ANY (?)";

        String insertBookingSQL =
                "INSERT INTO Booking (customer_phone, screening_id, seats_booked) " +
                        "VALUES (?, ?, ?) RETURNING booking_id";

        String insertSeatSQL =
                "INSERT INTO BookingSeat (booking_id, seat_id) VALUES (?, ?)";

        try (Connection conn = getConnection()) {
            conn.setAutoCommit(false);

            // 1️⃣ Check if seats are already booked
            try (PreparedStatement checkStmt = conn.prepareStatement(checkSeatsSQL)) {
                checkStmt.setInt(1, screeningId);
                checkStmt.setArray(2, conn.createArrayOf("INTEGER", seatIds.toArray()));

                ResultSet rs = checkStmt.executeQuery();
                List<Integer> alreadyBooked = new ArrayList<>();

                while (rs.next()) {
                    alreadyBooked.add(rs.getInt("seat_id"));
                }

                if (!alreadyBooked.isEmpty()) {
                    conn.rollback();
                    throw new SQLException("Seats already booked: " + alreadyBooked);
                }
            }

            // 2️⃣ Insert booking row
            int bookingId;
            try (PreparedStatement stmt = conn.prepareStatement(insertBookingSQL)) {
                stmt.setString(1, phone);
                stmt.setInt(2, screeningId);
                stmt.setInt(3, seatIds.size());

                ResultSet rs = stmt.executeQuery();
                rs.next();
                bookingId = rs.getInt("booking_id");
            }

            // 3️⃣ Insert seats
            try (PreparedStatement seatStmt = conn.prepareStatement(insertSeatSQL)) {
                for (int seatId : seatIds) {
                    seatStmt.setInt(1, bookingId);
                    seatStmt.setInt(2, seatId);
                    seatStmt.addBatch();
                }
                seatStmt.executeBatch();
            }

            conn.commit();
        }
    }

    @Override
    public void unBookSeat(int screeningId, String Phone) throws SQLException {

    }


}
