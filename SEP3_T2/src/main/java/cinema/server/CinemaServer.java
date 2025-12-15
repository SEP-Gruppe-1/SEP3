package cinema.server;

import cinema.model.Hall;
import cinema.persistence.*;
import io.grpc.Server;
import io.grpc.ServerBuilder;

public class CinemaServer
{
  private CustomerDAO customerDAO;
  private HallDAO hallDAO;
  private ScreeningDAO screeningDAO;
  private MovieDAO movieDAO;
  private LayoutDAO layoutDAO;
  private SeatDAO seatDAO;

  public static void main(String[] args) throws Exception
  {
    new CinemaServer().run();
  }

private void run() throws Exception
  {
    customerDAO = CustomerDAOImpl.getInstance();
    hallDAO = HallDAOImpl.getInstance();
    screeningDAO = ScreeningDAOImpl.getInstance();
    movieDAO = MovieDAOImpl.getInstance();
    layoutDAO = LayoutDAOImpl.getInstance();
    seatDAO = SeatDAOImpl.getInstance();

      System.out.println("DB screenings: " + screeningDAO.getAllScreenings().size());
      System.out.println("DB movies: " + movieDAO.getAllMovies().size());
      System.out.println("DB Halls "  + hallDAO.getAllHalls().size());
      System.out.println("i denne hall er det højste sæde "+ screeningDAO.getAllScreenings().get(0).getHall().getLayouts().getMaxLetter() + " " +screeningDAO.getAllScreenings().get(0).getHall().getLayouts().getMaxSeatInt()) ;
      System.out.println("DB poster_Url" + movieDAO.getMovieById(1).getPoster_url());
      System.out.println("DB banner_Url" + movieDAO.getMovieById(1).getBanner_url());

    Server server = ServerBuilder.forPort(9090)
        .addService(new CinemaServiceImpl(customerDAO, hallDAO, screeningDAO, movieDAO, layoutDAO, seatDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
