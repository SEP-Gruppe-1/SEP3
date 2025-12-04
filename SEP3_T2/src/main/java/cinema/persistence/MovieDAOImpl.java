package cinema.persistence;

import cinema.model.Movie;
import cinema.model.Screening;

import java.sql.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class MovieDAOImpl implements MovieDAO {

    public static MovieDAOImpl instance;


    public MovieDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
    }

    public static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
                "postgres", "123");

    }

    public static MovieDAOImpl getInstance() throws SQLException {
        if (instance == null) {
            instance = new MovieDAOImpl();
        }
        return instance;
    }


    @Override
    public Movie getMovieById(int id) {
        String sql = "SELECT * FROM Movie WHERE movie_id = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            try (ResultSet rs = stmt.executeQuery()) {
                if (rs.next()) {
                    return new Movie(
                            rs.getInt("movie_id"),
                            rs.getString("title"),
                            rs.getInt("duration_minutes"),
                            rs.getString("genre"),
                            rs.getDate("release_date").toLocalDate()

                    );
                }
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return null;
    }

    @Override
    public List<Movie> getAllMovies() {

        List<Movie> movies = new ArrayList<>();
        String sql = "SELECT * FROM Movie";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {


                movies.add(
                        new Movie(

                                rs.getInt("movie_id"),
                                rs.getString("title"),
                                rs.getInt("Duration_minutes"),
                                rs.getString("genre"),
                                rs.getDate("release_date").toLocalDate()

                        ));
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return movies;
    }
}

