package cinema.model;

public class Movie {

    public String title;
    public int playTime;
    public String genre;
    public String releaseDate;


    public Movie(String Title, int PlayTime, String Genre, String ReleaseDate) {
        this.title = Title;
        this.playTime = PlayTime;
        this.genre = Genre;
        this.releaseDate = ReleaseDate;
    }

    public Movie(){

    }

    public int getPlayTime() {
        return playTime;
    }

    public String getGenre() {
        return genre;
    }

    public String getReleaseDate() {
        return releaseDate;
    }

    public String getTitle() {
        return title;
    }

    public void setGenre(String genre) {
        this.genre = genre;
    }

    public void setPlayTime(int playTime) {
        this.playTime = playTime;
    }

    public void setReleaseDate(String releaseDate) {
        this.releaseDate = releaseDate;
    }

    public void setTitle(String title) {
        title = title;
    }


}

