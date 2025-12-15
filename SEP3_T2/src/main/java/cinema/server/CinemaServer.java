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
    System.out.println("Starting Cinema persistence server...");

    customerDAO = CustomerDAOImpl.getInstance();
    hallDAO = HallDAOImpl.getInstance();
    screeningDAO = ScreeningDAOImpl.getInstance();
    movieDAO = MovieDAOImpl.getInstance();
    layoutDAO = LayoutDAOImpl.getInstance();
    seatDAO = SeatDAOImpl.getInstance();

    if (customerDAO.getCustomerByPhone("12345678") == null)
    {
      customerDAO.createCustomer(
          new cinema.model.Customer("admin", "admin", "admin@cinema.com",
              "12345678", "Admin"));
      System.out.println("Admin user created with phone: 12345678 and password: admin");
    }

    Server server = ServerBuilder.forPort(9090).addService(
        new CinemaServiceImpl(customerDAO, hallDAO, screeningDAO, movieDAO,
            layoutDAO, seatDAO)).build();

    server.start();
    server.awaitTermination();
  }
}
