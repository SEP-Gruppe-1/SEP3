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

  public static void main(String[] args) throws Exception
  {
    new CinemaServer().run();
  }

private void run() throws Exception
  {
    customerDAO = CustomerDAOImpl.getInstance();
    hallDAO = HallDAOImpl.getInstance();
    screeningDAO = ScreeningDAOImpl.getInstance();

    Server server = ServerBuilder.forPort(9090)
        .addService(new CinemaServiceImpl(customerDAO, hallDAO, screeningDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
