package cinema.persistence;

import cinema.model.Layout;
import cinema.model.Movie;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class LayoutDAOImpl implements LayoutDAO {

    public static LayoutDAOImpl instance;

    private LayoutDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
    }

    private static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
                "postgres", "123");
    }

    public static LayoutDAOImpl getInstance() throws SQLException {
        if (instance == null) {
            instance = new LayoutDAOImpl();
        }
        return instance;
    }

    @Override
    public Layout getLayoutById(int layoutId) {
        String sql = "SELECT * FROM seat_layout WHERE layout_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, layoutId);
            try (ResultSet rs = stmt.executeQuery()) {
                if (rs.next()){

                          char row = rs.getString("Max_row_letter").charAt(0);
                          int number = rs.getInt("Max_seat_number");
                          int id =  rs.getInt("layout_id");

                          Layout.create(id, row, number);

                }
            }
        }
        catch (SQLException e)
        {
            System.out.println(e.toString());
        }
        return null;
    }

    @Override
    public List<Layout> getAllLayouts() {

        List<Layout> layouts = new ArrayList<>();
        String sql = "SELECT * FROM seat_layout";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {

                char row = rs.getString("Max_row_letter").charAt(0);
                int number = rs.getInt("Max_seat_number");
                int id =  rs.getInt("layout_id");


                layouts.add(Layout.create(id, row, number));

            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return layouts;
    }

}
