package cinema.server;

import cinema.dto.DTOFactory;
import cinema.model.*;
import cinema.persistence.*;
import grpccinema.*;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import org.mindrot.jbcrypt.BCrypt;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class CinemaServiceImpl extends CinemaServiceGrpc.CinemaServiceImplBase
{
  private final CustomerDAO customerDAO;
  private final HallDAO hallDAO;
  private final ScreeningDAO screeningDAO;
  private final MovieDAO movieDAO;
  private final LayoutDAO layoutDAO;
  private final SeatDAO seatDAO;

  public CinemaServiceImpl(CustomerDAO customerDAO, HallDAO hallDAO,
      ScreeningDAO screeningDAO, MovieDAO movieDAO, LayoutDAO layoutDAO,
      SeatDAO seatDAO)
  {
    this.customerDAO = customerDAO;
    this.hallDAO = hallDAO;
    this.screeningDAO = screeningDAO;
    this.movieDAO = movieDAO;
    this.layoutDAO = layoutDAO;
    this.seatDAO = seatDAO;
  }

  // Hjælpemetode til error håndtering
  private void handleError(StreamObserver<?> observer, Status status,
      String description, Throwable t)
  {
    Status s = status.withDescription(description);
    if (t != null)
    {
      s = s.withCause(t);
    }
    observer.onError(s.asRuntimeException());
  }

  @Override public void getCustomers(GetCustomersRequest request,
      StreamObserver<GetCustomersResponse> responseObserver)
  {
    try
    {
      GetCustomersResponse response = DTOFactory.createGetCustomersResponse(
          customerDAO.getAllCustomers());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get customers: " + e.getMessage(), e);
    }
  }

  @Override public void getHalls(GetHallsRequest request,
      StreamObserver<GetHallsResponse> responseObserver)
  {
    try
    {
      GetHallsResponse response = DTOFactory.createGetHallResponse(
          hallDAO.getAllHalls());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (SQLException e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get halls: " + e.getMessage(), e);
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Unexpected error while getting halls: " + e.getMessage(), e);
    }
  }

  @Override public void getCustomerByPhone(GetCustomerByPhoneRequest request,
      StreamObserver<GetCustomerByPhoneResponse> responseObserver)
  {
    try
    {
      Customer customer = customerDAO.getCustomerByPhone(request.getPhone());
      if (customer == null)
      {
        handleError(responseObserver, Status.NOT_FOUND,
            "Customer not found with phone: " + request.getPhone(), null);
        return;
      }
      DTOCustomer dtoCustomer = DTOFactory.createDTOCustomer(customer);
      GetCustomerByPhoneResponse response = GetCustomerByPhoneResponse.newBuilder()
          .setCustomer(dtoCustomer).build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get customer: " + e.getMessage(), e);
    }
  }

  @Override public void saveMovie(SaveMovieRequest request,
      StreamObserver<SaveMovieResponse> responseObserver)
  {
    try
    {
      DTOMovie dto = request.getMovie();
      Movie movie = DTOFactory.createMovie(dto);
      Movie existing = movieDAO.getMovieById(movie.getId());

      if (existing == null)
      {
        movieDAO.AddMovie(movie);
      }
      DTOMovie savedDto = DTOFactory.createDTOMovie(movie);

      SaveMovieResponse response = SaveMovieResponse.newBuilder()
          .setMovie(savedDto).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not save movie: " + e.getMessage(), e);
    }
  }

  @Override public void saveCustomer(SaveCustomerRequest request,
      StreamObserver<SaveCustomerResponse> responseObserver)
  {
    try
    {
      DTOCustomer dto = request.getCustomer();
      Customer customer = DTOFactory.createCustomer(dto);

      Customer existing = customerDAO.getCustomerByPhone(customer.getPhone());

      if (existing == null)
      {
        customerDAO.createCustomer(customer);
      }
      else
      {
        customerDAO.updateCustomerRole(customer.getPhone(), customer.getRole());
      }

      DTOCustomer savedDto = DTOFactory.createDTOCustomer(customer);

      responseObserver.onNext(
          SaveCustomerResponse.newBuilder().setCustomer(savedDto).build());
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not save customer: " + e.getMessage(), e);
    }
  }

  @Override public void deleteCustomer(DeleteCustomerRequest request,
      StreamObserver<DeleteCustomerResponse> responseObserver)
  {
    try
    {
      String phone = request.getPhone();

      Customer existing = customerDAO.getCustomerByPhone(phone);

      if (existing == null)
      {
        handleError(responseObserver, Status.NOT_FOUND,
            "Customer not found with phone: " + phone, null);
        return;
      }

      customerDAO.deleteCustomerByPhone(phone);

      DeleteCustomerResponse response = DeleteCustomerResponse.newBuilder()
          .setPhone(phone).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not delete customer: " + e.getMessage(), e);
    }
  }

  @Override public void deleteBooking(DeleteBookingRequest request,
      StreamObserver<DeleteBookingResponse> responseObserver)
  {
    String phoneNumber = request.getPhoneNumber();
    int screeningId = request.getScreeningId();

    try
    {
      seatDAO.DeleteBooking(screeningId, phoneNumber);

      DeleteBookingResponse response = DeleteBookingResponse.newBuilder()
          .build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (SQLException e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not delete booking: " + e.getMessage(), e);
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Unexpected error deleting booking: " + e.getMessage(), e);
    }
  }

  @Override public void verifyCustomerPassword(
      VerifyCustomerPasswordRequest request,
      StreamObserver<VerifyCustomerPasswordResponse> responseObserver)
  {
    try
    {
      String phone = request.getPhone();
      String password = request.getPassword();

      Customer customer = customerDAO.getCustomerByPhone(phone);
      boolean isValid = false;
      if (customer != null && customer.getPassword() != null)
      {
        isValid = BCrypt.checkpw(password, customer.getPassword());
      }

      VerifyCustomerPasswordResponse response = VerifyCustomerPasswordResponse.newBuilder()
          .setIsValid(isValid).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not verify password: " + e.getMessage(), e);
    }
  }

  @Override public void getHallByID(GetHallByIdRequest request,
      StreamObserver<GetHallByIdResponse> responseObserver)
  {
    try
    {
      Hall hall = hallDAO.getHallById(request.getId());
      if (hall == null)
      {
        handleError(responseObserver, Status.NOT_FOUND,
            "Hall not found with id: " + request.getId(), null);
        return;
      }
      DTOHall dtoHall = DTOFactory.createDTOHall(hall);
      GetHallByIdResponse response = GetHallByIdResponse.newBuilder()
          .setHall(dtoHall).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (SQLException e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get hall: " + e.getMessage(), e);
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Unexpected error while getting hall: " + e.getMessage(), e);
    }
  }

  @Override public void getAllScreenings(GetAllScreeningsRequest request,
      StreamObserver<GetAllScreeningsResponse> responseObserver)
  {
    try
    {
      GetAllScreeningsResponse response = DTOFactory.createGetAllScreeningsResponse(
          screeningDAO.getAllScreenings());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get screenings: " + e.getMessage(), e);
    }
  }

  @Override public void getScreeningByID(GetScreeningByIdRequest request,
      StreamObserver<GetScreeningByIdResponse> responseObserver)
  {
    try
    {
      Screening screening = screeningDAO.getScreeningById(request.getId());
      if (screening == null)
      {
        handleError(responseObserver, Status.NOT_FOUND,
            "Screening not found with id: " + request.getId(), null);
        return;
      }
      DTOScreening dtoScreening = DTOFactory.createDTOScreening(screening);
      GetScreeningByIdResponse response = GetScreeningByIdResponse.newBuilder()
          .setScreening(dtoScreening).build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get screening: " + e.getMessage(), e);
    }
  }

  @Override public void saveScreening(SaveScreeningRequest request,
      StreamObserver<SaveScreeningResponse> responseObserver)
  {
    try
    {
      DTOScreening dto = request.getScreening();
      Screening screening = DTOFactory.createScreening(dto);

      // Hvis client giver et id, tjek om den allerede findes. Ellers opret ny.
      Screening existing = null;
      if (screening.getScreeningId() != 0)
      {
        existing = screeningDAO.getScreeningById(screening.getScreeningId());
      }

      if (existing == null)
      {
        int newId = screeningDAO.addScreening(screening);
        screening.setScreeningId(newId);
      }

      DTOScreening savedDto = DTOFactory.createDTOScreening(screening);

      SaveScreeningResponse response = SaveScreeningResponse.newBuilder()
          .setScreening(savedDto).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();

    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not create screening: " + e.getMessage(), e);
    }
  }

  @Override public void getMovieByID(GetMovieByIdRequest request,
      StreamObserver<GetMovieByIdResponse> responseObserver)
  {
    try
    {
      Movie movie = movieDAO.getMovieById(request.getId());
      if (movie == null)
      {
        handleError(responseObserver, Status.NOT_FOUND,
            "Movie not found with id: " + request.getId(), null);
        return;
      }
      DTOMovie dtoMovie = DTOFactory.createDTOMovie(movie);
      GetMovieByIdResponse response = GetMovieByIdResponse.newBuilder()
          .setMovie(dtoMovie).build();

      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get movie: " + e.getMessage(), e);
    }
  }

  @Override public void getAllMovies(GetAllMoviesRequest request,
      StreamObserver<GetAllMoviesResponse> responseObserver)
  {
    try
    {
      GetAllMoviesResponse response = DTOFactory.createGetAllMoviesResponse(
          movieDAO.getAllMovies());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get movies: " + e.getMessage(), e);
    }
  }

  @Override public void getAllLayouts(GetAllLayoutsRequest request,
      StreamObserver<GetAllLayoutsResponse> responseObserver)
  {
    try
    {
      GetAllLayoutsResponse response = DTOFactory.createGetAllLayoutsResponse(
          layoutDAO.getAllLayouts());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get layouts: " + e.getMessage(), e);
    }
  }

  @Override public void getAllSeats(GetAllSeatsRequest request,
      StreamObserver<GetAllSeatsResponse> responseObserver)
  {
    try
    {
      GetAllSeatsResponse response = DTOFactory.createGetAllSeatsResponse(
          seatDAO.getAllSeats());
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get seats: " + e.getMessage(), e);
    }
  }

  @Override public void getSeatsByScreening(GetSeatsByScreeningRequest request,
      StreamObserver<GetSeatsByScreeningResponse> responseObserver)
  {
    try
    {
      List<Seat> seats = seatDAO.getSeatsByScreening(request.getScreeningId());
      if (seats == null || seats.isEmpty())
      {
        // Tom men valid response -> returner tom liste
        GetSeatsByScreeningResponse emptyResponse = GetSeatsByScreeningResponse.newBuilder()
            .build();
        responseObserver.onNext(emptyResponse);
        responseObserver.onCompleted();
        return;
      }

      List<DTOSeat> dtoSeats = new ArrayList<>();
      for (Seat seat : seats)
      {
        dtoSeats.add(DTOFactory.createDTOSeat(seat));
      }

      GetSeatsByScreeningResponse response = GetSeatsByScreeningResponse.newBuilder()
          .addAllSeats(dtoSeats).build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (SQLException e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Could not get seats: " + e.getMessage(), e);
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Unexpected error while getting seats: " + e.getMessage(), e);
    }
  }

  @Override public void bookSeats(BookSeatsRequest request,
      StreamObserver<BookSeatsResponse> responseObserver)
  {
    int screeningId = request.getScreeningId();
    String customerPhone = request.getCustomerPhone();
    List<Integer> seatIds = request.getSeatIdsList();

    try
    {
      seatDAO.bookSeat(screeningId, customerPhone, seatIds);

      BookSeatsResponse response = BookSeatsResponse.newBuilder().build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Booking failed: " + e.getMessage(), e);
    }
  }

  @Override public void updateBooking(UpdateBookingRequest request,
      StreamObserver<UpdateBookingResponse> responseObserver)
  {
    int screeningId = request.getScreeningId();
    String customerPhone = request.getCustomerPhone();
    List<Integer> seatsToAdd = request.getSeatsToAddList();
    List<Integer> seatsToRemove = request.getSeatsToRemoveList();

    try
    {
      seatDAO.updateBooking(screeningId, customerPhone, seatsToAdd,
          seatsToRemove);

      UpdateBookingResponse response = UpdateBookingResponse.newBuilder()
          .setSuccess(true).setMessage("Booking updated").build();
      responseObserver.onNext(response);
      responseObserver.onCompleted();
    }
    catch (Exception e)
    {
      handleError(responseObserver, Status.INTERNAL,
          "Booking update failed: " + e.getMessage(), e);
    }
  }
}
