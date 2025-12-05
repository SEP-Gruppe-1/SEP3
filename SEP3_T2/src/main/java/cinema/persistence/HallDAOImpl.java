package cinema.persistence;

import cinema.model.Customer;
import cinema.model.Hall;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class HallDAOImpl implements HallDAO {


    public static HallDAOImpl instance;
    public LayoutDAO layoutDAO = LayoutDAOImpl.getInstance();
    public SeatDAO seatDAO = SeatDAOImpl.getInstance();


    private HallDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
        LayoutDAO layoutDAO = LayoutDAOImpl.getInstance();

    }

    private static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
                "postgres", "123");
    }

    public static HallDAOImpl getInstance() throws SQLException {
        if (instance == null) {
            instance = new HallDAOImpl();
        }
        return instance;
    }

    @Override
    public Hall getHallById(int id) throws SQLException {

        layoutDAO.getAllLayouts();

        seatDAO.getAllSeats();
        String sql = "select * from hall where hall_id = ?";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            try (ResultSet rs = stmt.executeQuery()) {

                if (rs.next()) {
                    return Hall.getInstance(rs.getInt("hall_id"));
                }
            }


        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return null;
    }

    @Override
    public List<Hall> getAllHalls() throws SQLException {

        layoutDAO.getAllLayouts();

        seatDAO.getAllSeats();
        List<Hall> halls = new ArrayList<>();
        String sql = "SELECT hall_id, hall_number, layout_id FROM Hall";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                halls.add(
                        Hall.getInstance(rs.getInt("hall_id")));
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return halls;
    }
}
