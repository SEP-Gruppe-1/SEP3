package cinema.persistence;

import cinema.model.Movie;

public interface MovieDAO {
    Movie getMovieById(int movieId);
}
