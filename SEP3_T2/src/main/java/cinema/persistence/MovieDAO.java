package cinema.persistence;

import cinema.model.Movie;

import java.util.List;

public interface MovieDAO {
    Movie getMovieById(int movieId);
    List<Movie> getAllMovies();
    Movie AddMovie(Movie movie);

}
