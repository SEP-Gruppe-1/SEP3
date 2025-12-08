package cinema.model;

import java.time.LocalDate;
import java.util.Date;

public class Movie {

    public String title;
    public int playTime;
    public String genre;
    public LocalDate releaseDate;
    public int id;


    public Movie(int id, String Title, int PlayTime, String Genre, LocalDate ReleaseDate) {
        this.id = id;
        this.title = Title;
        this.playTime = PlayTime;
        this.genre = Genre;
        this.releaseDate = ReleaseDate;
    }

    public Movie(){

    }

    public int getId() {
        return id;
    }

    public int getPlayTime() {
        return playTime;
    }

    public String getGenre() {
        return genre;
    }

    public LocalDate getReleaseDate() {
        return releaseDate;
    }

    public String getTitle() {
        return title;
    }

    public void setId(int id) {this.id = id;}

    public void setGenre(String genre) {
        this.genre = genre;
    }

    public void setPlayTime(int playTime) {
        this.playTime = playTime;
    }

    public void setReleaseDate(LocalDate releaseDate) {
        this.releaseDate = releaseDate;
    }

    public void setTitle(String title) {
        title = title;
    }


}

