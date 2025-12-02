package cinema.server;

import cinema.dto.DTOFactory;
import cinema.model.Customer;
import cinema.model.Hall;
import cinema.model.Movie;
import cinema.model.Screening;
import cinema.persistence.*;
import grpccinema.*;
import io.grpc.stub.StreamObserver;

import java.sql.SQLException;

public class CinemaServiceImpl extends CinemaServiceGrpc.CinemaServiceImplBase
{
  private CustomerDAO customerDAO;
  private HallDAO hallDAO;
  private ScreeningDAO screeningDAO;
  private MovieDAO movieDAO;

  public CinemaServiceImpl(CustomerDAO customerDAO, HallDAO hallDAO, ScreeningDAO screeningDAO,  MovieDAO movieDAO)
  {
    this.customerDAO = customerDAO;
    this.hallDAO = hallDAO;
    this.screeningDAO = screeningDAO;
    this.movieDAO = movieDAO;

  }

  @Override public void getCustomers(GetCustomersRequest request,
      StreamObserver<GetCustomersResponse> responseObserver)
  {
    GetCustomersResponse response = DTOFactory.createGetCustomersResponse(
        customerDAO.getAllCustomers());

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

@Override
    public void getHalls(GetHallsRequest request, StreamObserver<GetHallsResponse> responseObserver)  {
        GetHallsResponse response = DTOFactory.createGetHallResponse(hallDAO.getAllHalls());



        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

  @Override public void getCustomerByPhone(GetCustomerByPhoneRequest request,
      StreamObserver<GetCustomerByPhoneResponse> responseObserver)
  {
    Customer customer = customerDAO.getCustomerByPhone(request.getPhone());
    DTOCustomer dtoCustomer = DTOFactory.createDTOCustomer(customer);
    GetCustomerByPhoneResponse response = GetCustomerByPhoneResponse.newBuilder()
        .setCustomer(dtoCustomer).build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

  @Override public void saveCustomer(SaveCustomerRequest request,
      StreamObserver<SaveCustomerResponse> responseObserver)  {
      try {

          DTOCustomer dto = request.getCustomer();
          Customer customer = DTOFactory.createCustomer(dto);
          Customer existing = customerDAO.getCustomerByPhone(customer.getPhone());

          if (existing == null) {
              customerDAO.createCustomer(customer);
          } else {
              customerDAO.updateCustomer(customer);
          }
          DTOCustomer savedDto = DTOFactory.createDTOCustomer(customer);

          SaveCustomerResponse response = SaveCustomerResponse.newBuilder()
                  .setCustomer(savedDto)
                  .build();

          responseObserver.onNext(response);
          responseObserver.onCompleted();

      } catch (Exception e) {
          e.printStackTrace();
          responseObserver.onError(
                  io.grpc.Status.INTERNAL
                          .withDescription("Could not save customer: " + e.getMessage())
                          .asRuntimeException()
          );
      }
  }

    @Override
    public void getHallByID(GetHallByIdRequest request, StreamObserver<GetHallByIdResponse> responseObserver) {

            Hall hall = hallDAO.getHallById(request.getId());
            DTOHall dtoHall = DTOFactory.createDTOHall(hall);
            GetHallByIdResponse response = GetHallByIdResponse.newBuilder()
                    .setHall(dtoHall).build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();

    }

    @Override
    public void getAllScreenings(GetAllScreeningsRequest request, StreamObserver<GetAllScreeningsResponse> responseObserver) {
        GetAllScreeningsResponse respones = DTOFactory.createGetAllScreeningsResponse(screeningDAO.getAllScreenings());

        responseObserver.onNext(respones);
        responseObserver.onCompleted();
    }

    @Override
    public void getScreeningByID(GetScreeningByIdRequest request, StreamObserver<GetScreeningByIdResponse> responseObserver) {
        Screening screening = screeningDAO.getScreeningById(request.getId());
        DTOScreening dtoScreening = DTOFactory.createDTOScreening(screening);
        GetScreeningByIdResponse response = GetScreeningByIdResponse.newBuilder()
                .setScreening(dtoScreening).build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getMovieByID(GetMovieByIdRequest request, StreamObserver<GetMovieByIdResponse> responseObserver) {

      Movie movie = movieDAO.getMovieById(request.getId());
        DTOMovie dtoMovie = DTOFactory.createDTOMovie(movie);
        GetMovieByIdResponse response = GetMovieByIdResponse.newBuilder()
                .setMovie(dtoMovie).build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getAllMovies(GetAllMoviesRequest request, StreamObserver<GetAllMoviesResponse> responseObserver) {
        GetAllMoviesResponse response = DTOFactory.createGetAllMoviesResponse(movieDAO.getAllMovies());
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }
}
