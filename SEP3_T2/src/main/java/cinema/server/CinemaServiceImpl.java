package cinema.server;

import cinema.dto.DTOFactory;
import cinema.model.*;
import cinema.persistence.*;
import grpccinema.*;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class CinemaServiceImpl extends CinemaServiceGrpc.CinemaServiceImplBase {
    private CustomerDAO customerDAO;
    private HallDAO hallDAO;
    private ScreeningDAO screeningDAO;
    private MovieDAO movieDAO;
    private LayoutDAO layoutDAO;
    private SeatDAO seatDAO;

    public CinemaServiceImpl(CustomerDAO customerDAO, HallDAO hallDAO, ScreeningDAO screeningDAO, MovieDAO movieDAO, LayoutDAO layoutDAO, SeatDAO seatDAO) {
        this.customerDAO = customerDAO;
        this.hallDAO = hallDAO;
        this.screeningDAO = screeningDAO;
        this.movieDAO = movieDAO;
        this.layoutDAO = layoutDAO;
        this.seatDAO = seatDAO;

    }

    @Override
    public void getCustomers(GetCustomersRequest request,
                             StreamObserver<GetCustomersResponse> responseObserver) {
        GetCustomersResponse response = DTOFactory.createGetCustomersResponse(
                customerDAO.getAllCustomers());

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getHalls(GetHallsRequest request, StreamObserver<GetHallsResponse> responseObserver) {
        GetHallsResponse response = null;
        try {
            response = DTOFactory.createGetHallResponse(hallDAO.getAllHalls());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }


        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getCustomerByPhone(GetCustomerByPhoneRequest request,
                                   StreamObserver<GetCustomerByPhoneResponse> responseObserver) {
        Customer customer = customerDAO.getCustomerByPhone(request.getPhone());
        DTOCustomer dtoCustomer = DTOFactory.createDTOCustomer(customer);
        GetCustomerByPhoneResponse response = GetCustomerByPhoneResponse.newBuilder()
                .setCustomer(dtoCustomer).build();

        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void saveCustomer(SaveCustomerRequest request,
                             StreamObserver<SaveCustomerResponse> responseObserver) {
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
    public void deleteCustomer(DeleteCustomerRequest request,
                               StreamObserver<DeleteCustomerResponse> responseObserver) {
        try {
            String phone = request.getPhone();

            System.out.println("Deleting customer with phone: " + phone);

            Customer existing = customerDAO.getCustomerByPhone(phone);

            if (existing == null) {
                responseObserver.onError(
                        Status.NOT_FOUND
                                .withDescription("Customer not found with phone: " + phone)
                                .asRuntimeException()
                );
                return;
            }

            customerDAO.deleteCustomerByPhone(phone);

            DeleteCustomerResponse response = DeleteCustomerResponse.newBuilder()
                    .setPhone(phone)
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();

        } catch (Exception e) {
            e.printStackTrace();
            responseObserver.onError(
                    Status.INTERNAL
                            .withDescription("Could not delete customer: " + e.getMessage())
                            .asRuntimeException()
            );
        }
    }





    @Override
    public void getHallByID(GetHallByIdRequest request, StreamObserver<GetHallByIdResponse> responseObserver) {

        Hall hall = null;
        try {
            hall = hallDAO.getHallById(request.getId());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
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


    @Override
    public void getAllLayouts(GetAllLayoutsRequest request, StreamObserver<GetAllLayoutsResponse> responseObserver) {
        GetAllLayoutsResponse response = DTOFactory.createGetAllLayoutsResponse(layoutDAO.getAllLayouts());
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }


    @Override
    public void getAllSeats(GetAllSeatsRequest request, StreamObserver<GetAllSeatsResponse> responseObserver) {
        GetAllSeatsResponse response = DTOFactory.createGetAllSeatsResponse(seatDAO.getAllSeats());
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void getSeatsByScreening(GetSeatsByScreeningRequest request, StreamObserver<GetSeatsByScreeningResponse> responseObserver) {
        List<DTOSeat> dtoSeats = new ArrayList<>();
        try {
            List<Seat> seats = seatDAO.getSeatsByScreening(request.getScreeningId());

            for (Seat seat : seats) {
                DTOSeat dtoSeat = DTOFactory.createDTOSeat(seat);
                dtoSeats.add(dtoSeat);


            }
        }
        catch (SQLException e) {
            throw new RuntimeException(e);
        }

        GetSeatsByScreeningResponse response = GetSeatsByScreeningResponse.newBuilder().
                addAllSeats(dtoSeats).build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();

    }

    @Override
    public void bookSeats(BookSeatsRequest request, StreamObserver<BookSeatsResponse> responseObserver) {

        int screeningId = request.getScreeningId();
        String customerPhone = request.getCustomerPhone();
        List<Integer> seatIds = request.getSeatIdsList();

        try {
            seatDAO.bookSeat(screeningId, customerPhone, seatIds);

            // Tom response-body
            BookSeatsResponse response = BookSeatsResponse.newBuilder().build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
        catch (Exception e) {
            e.printStackTrace();

            responseObserver.onError(
                    Status.INTERNAL
                            .withDescription("Booking failed: " + e.getMessage())
                            .asRuntimeException()
            );
        }

    }
}
