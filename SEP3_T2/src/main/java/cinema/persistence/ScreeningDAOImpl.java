package cinema.persistence;

import cinema.model.Hall;
import cinema.model.Movie;
import cinema.model.Screening;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;


public class ScreeningDAOImpl implements ScreeningDAO {

    public static ScreeningDAOImpl instance;
    public static MovieDAO movieInstance;
    public  static LayoutDAO layoutInstance;


    public ScreeningDAOImpl(MovieDAO movieDAO) throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
        this.movieInstance = movieDAO;
        this.layoutInstance = new LayoutDAOImpl();
    }

    public static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
                "postgres", "123");

    }

    public static ScreeningDAOImpl getInstance() throws SQLException {
        if (instance == null) {
            MovieDAO movieInstance = MovieDAOImpl.getInstance();
            instance = new ScreeningDAOImpl(movieInstance);
        }
        return instance;
    }

    @Override
    public List<Screening> getAllScreenings() {
        List<Screening> screenings = new ArrayList<>();
        String sql = "SELECT * FROM Screening";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                layoutInstance.getLayoutById(rs.getInt("hall_id"));

                int movieId = rs.getInt("movie_id");
                Movie movie = movieInstance.getMovieById(movieId);

                screenings.add(
                        new Screening(
                                movie,
                                rs.getInt("Hall_id"),
                                rs.getTime("Start_time").toLocalTime(),
                                rs.getDate("screening_date").toLocalDate(),
                                rs.getInt("available_seats"),
                                rs.getInt("screening_id")
                        ));
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return screenings;
    }


    @Override
    public Screening getScreeningById(int id) {
        String sql = "SELECT * FROM screening WHERE screening_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            try (ResultSet rs = stmt.executeQuery()) {
                if (rs.next()) {
                    int movieId = rs.getInt("movie_id");
                    Movie movie = movieInstance.getMovieById(movieId);

                    return new Screening(

                            movie,
                            rs.getInt("Hall_id"),
                            rs.getTime("Start_time").toLocalTime(),
                            rs.getDate("screening_date").toLocalDate(),
                            rs.getInt("available_seats"),
                            rs.getInt("screening_id")

                    );
                }
            }


        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return null;

    }

    @Override
    public void addScreening(Screening screening) throws SQLException {
        String sql = "INSERT INTO screening (screening_id, movie_id, hall_id, screening_date, start_time, available_seats) VALUES (?, ?, ?, ?, ?, ?)";
        try (Connection conn = getConnection()){
            PreparedStatement stmt = conn.prepareStatement(sql);
            stmt.setInt(1, screening.getScreeningId());
            stmt.setInt(2,screening.getMovie().getId());
            stmt.setInt(3,screening.getHall().getId());
            stmt.setDate(4, Date.valueOf(screening.getDate()));
            stmt.setTime(5, Time.valueOf(screening.getStartTime()));
            stmt.setInt(6, screening.getAvailableSeats());
            stmt.executeUpdate();
        }
    }


}

