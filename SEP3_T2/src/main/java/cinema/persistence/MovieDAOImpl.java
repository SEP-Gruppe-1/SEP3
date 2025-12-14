package cinema.persistence;

import cinema.model.Movie;
import cinema.model.Screening;

import java.sql.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class MovieDAOImpl implements MovieDAO {

    public static MovieDAOImpl instance;


    private MovieDAOImpl() throws SQLException {
        DriverManager.registerDriver(new org.postgresql.Driver());
    }

    private static Connection getConnection() throws SQLException {
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
    public Movie AddMovie(Movie movie) {
        String sql = "INSERT INTO Movie (title, duration_minutes, genre, release_date, description, poster_url, banner_url) VALUES (?, ?, ?, ?, ?, ?, ?)";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            stmt.setString(1, movie.getTitle());
            stmt.setInt(2, movie.getPlayTime());
            stmt.setString(3, movie.getGenre());
            stmt.setDate(4, Date.valueOf(movie.getReleaseDate()));
            stmt.setString(5, movie.getDescription());
            stmt.setString(6, movie.getPoster_url());
            stmt.setString(7, movie.getBanner_url());

            int affectedRows = stmt.executeUpdate();

            if (affectedRows == 0) {
                throw new SQLException("Creating movie failed, no rows affected.");
            }

            try (ResultSet generatedKeys = stmt.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    movie.setId(generatedKeys.getInt(1));
                } else {
                    throw new SQLException("Creating movie failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return movie;
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
                            rs.getDate("release_date").toLocalDate(),
                            rs.getString("description"),
                            rs.getString("poster_url"),
                            rs.getString("banner_url")

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
                                rs.getDate("release_date").toLocalDate(),
                                rs.getString("description"),
                                rs.getString("poster_url"),
                                rs.getString("banner_url")


                        ));
            }
        } catch (SQLException e) {
            System.out.println(e.toString());
        }
        return movies;
    }
}

