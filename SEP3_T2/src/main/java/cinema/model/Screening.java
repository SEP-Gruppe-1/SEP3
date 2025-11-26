package cinema.model;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class Screening {

    public Movie movie;
    public Hall hall;
    public List<Seat> availableSeats;
    public int screeningID;


    public Screening(Movie movie, Hall hall) {
        this.movie = movie;
        this.hall = hall;
        this.availableSeats = new ArrayList<>();
        this.screeningID = 0;
    }

}
