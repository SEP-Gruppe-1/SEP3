package cinema.model;

import java.time.LocalDate;
import java.util.Date;

public class Movie {

    public String title;
    public int playTime;
    public String genre;
    public LocalDate releaseDate;
    public int id;
    public String description;
    public String poster_url;
    public String banner_url;


    public Movie(int id, String Title, int PlayTime, String Genre, LocalDate ReleaseDate, String description, String poster_url, String banner_url) {
        this.id = id;
        this.title = Title;
        this.playTime = PlayTime;
        this.genre = Genre;
        this.releaseDate = ReleaseDate;
        this.description = description;
        this.poster_url = poster_url;
        this.banner_url = banner_url;
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

    public String getDescription() {return description;}

    public String getPoster_url() {return poster_url;}

    public String getBanner_url() {return banner_url;}

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
        this.title = title;
    }

    public void setDescription(String description) {this.description = description;}

    public void setPoster_url(String poster_url) {this.poster_url = poster_url;}

    public void setBanner_url(String banner_url) {this.banner_url = banner_url;}


}

