package cinema.server;

import cinema.persistence.CustomerDAO;
import cinema.persistence.CustomerDAOImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;

public class CinemaServer
{
  private CustomerDAO customerDAO;

  public static void main(String[] args) throws Exception
  {
    new CinemaServer().run();
  }

private void run() throws Exception
  {
    CustomerDAO animalDAO = CustomerDAOImpl.getInstance();

    Server server = ServerBuilder.forPort(9090)
        .addService(new CinemaServiceImpl(customerDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
