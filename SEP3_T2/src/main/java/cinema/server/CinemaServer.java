package cinema.server;

import cinema.model.Hall;
import cinema.persistence.CustomerDAO;
import cinema.persistence.CustomerDAOImpl;
import cinema.persistence.HallDAO;
import cinema.persistence.HallDAOImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;

public class CinemaServer
{
  private CustomerDAO customerDAO;
  private HallDAO hallDAO;

  public static void main(String[] args) throws Exception
  {
    new CinemaServer().run();
  }

private void run() throws Exception
  {
    customerDAO = CustomerDAOImpl.getInstance();
    hallDAO = HallDAOImpl.getInstance();

    Server server = ServerBuilder.forPort(9090)
        .addService(new CinemaServiceImpl(customerDAO, hallDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
