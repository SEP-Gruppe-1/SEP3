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


    Server server = ServerBuilder.forPort(9090)
        .addService(new CinemaServiceImpl(customerDAO, hallDAO, screeningDAO, movieDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
