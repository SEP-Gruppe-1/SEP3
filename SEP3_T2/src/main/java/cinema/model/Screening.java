package cinema.model;




import java.time.LocalDate;
import java.time.LocalTime;

import java.util.Date;


public class Screening {

    public Movie movie;
    public Hall hall;
    public int hallId;
    public int availableSeats;
    public int screeningId;
    public LocalTime endTime;
    public LocalTime startTime;
    public LocalDate date;


    public Screening(Movie movie, int hallId, LocalTime startTime, LocalDate date, int availableSeats, int  screeningId) {
        this.movie = movie;
        this.hallId = hallId;
        this.hall =createHall();
        this.startTime = startTime;
        endTime= startTime.plusMinutes(movie.getPlayTime()+15);
        this.date = date;
        this.availableSeats = availableSeats;
        this.screeningId = screeningId;

    }

    public Screening(Movie movie, int hallId, int startTimeInInt, LocalDate date, int availableSeats, int  screeningId) {
        this.movie = movie;
        this.hallId = hallId;
        this.hall =createHall();
        this.startTime = LocalTime.of(0,0).plusMinutes(startTimeInInt);
        endTime= startTime.plusMinutes(movie.getPlayTime()+15);
        this.date = date;
        this.availableSeats = availableSeats;
        this.screeningId = screeningId;

    }

    public Hall createHall() {
       return  Hall.getInstance(hallId);
    }

    public LocalDate getDate() {
        return date;
    }

    public Hall getHall() {
        return hall;
    }

    public int getAvailableSeats() {
        return availableSeats;
    }

    public int getHallId() {
        return hallId;
    }

    public int getScreeningId() {
        return screeningId;
    }

    public LocalTime getEndTime() {
        return endTime;
    }

    public LocalTime getStartTime() {
        return startTime;
    }

    public Movie getMovie() {
        return movie;
    }

    public void setAvailableSeats(int availableSeats) {
        this.availableSeats = availableSeats;
    }

    public void setDate(LocalDate date) {
        this.date = date;
    }

    public void setEndTime(LocalTime endTime) {
        this.endTime = endTime;
    }

    public void setHall(Hall hall) {
        this.hall = hall;
    }

    public void setMovie(Movie movie) {
        this.movie = movie;
    }

    @Override
    public String toString() {
        return(getHall().getId() + " " + getDate() + " " + getStartTime() + " " + getEndTime() + " " + getAvailableSeats() + " " + getScreeningId() + " "+ getHallId() +" "+ getMovie().getTitle());
    }
}
