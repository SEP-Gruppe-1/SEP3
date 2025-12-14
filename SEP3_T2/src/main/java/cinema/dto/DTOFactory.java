package cinema.dto;

import cinema.model.*;
import grpccinema.*;


import java.time.LocalDate;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

public class DTOFactory {


    //---------- Customer ----------\\

    public static DTOCustomer createDTOCustomer(Customer customer) {
        if (customer == null) {
            return null; // returnér null hvis ingen har booket sædet
        }

        return DTOCustomer.newBuilder().setName(customer.getName())
                .setPassword(customer.getPassword()).setEmail(customer.getEmail())
                .setPhone(customer.getPhone()).setRole(customer.getRole()).build();
    }

  public static Customer createCustomer(DTOCustomer dtoCustomer)
  {
    return new Customer(dtoCustomer.getName(), dtoCustomer.getPassword(),
        dtoCustomer.getEmail(), dtoCustomer.getPhone(), dtoCustomer.getRole());
  }

    public static Customer createCustomer(GetCustomersResponse r) {
        return createCustomer(r.getCustomers(0));
    }

    public static Customer[] createCustomers(GetCustomersResponse r) {
        Customer[] res = new Customer[r.getCustomersCount()];
        for (int i = 0; i < r.getCustomersCount(); i++)
            res[i] = createCustomer(r.getCustomers(i));
        return res;
    }

    public static GetCustomersRequest createGetCustomersRequest() {
        return GetCustomersRequest.newBuilder().build();
    }

    public static GetCustomersResponse createGetCustomersResponse(
            List<Customer> customers) {

        ArrayList<DTOCustomer> dtoCustomers = new ArrayList<>();

        for (Customer c : customers) {
            dtoCustomers.add(
                    DTOCustomer.newBuilder()
                            .setName(c.getName())
                            .setPassword(c.getPassword())
                            .setEmail(c.getEmail())
                            .setPhone(c.getPhone())
                            .setRole(c.getRole())   // ✅ DEN AFGØRENDE LINJE
                            .build()
            );
        }

        return GetCustomersResponse.newBuilder()
                .addAllCustomers(dtoCustomers)
                .build();
    }


    //---------- Hall ----------\\

    public static DTOHall createDTOHall(Hall hall) {
        return DTOHall.newBuilder().setId(hall.getId()).setLayout(hall.getLayout())
                .setNumber(hall.getNumber()).build();
    }

    public static Hall createHall(DTOHall dtoHall) {
        return Hall.getInstance(dtoHall.getId());
    }

    public static Hall createHall(GetHallsResponse r) {
        return createHall(r.getHalls(0));
    }

    public static GetHallsRequest createGetHallRequest() {
        return GetHallsRequest.newBuilder().build();
    }

    public static GetHallsResponse createGetHallResponse(List<Hall> allHalls) {
        GetHallsResponse.Builder builder = GetHallsResponse.newBuilder();

        for (Hall hall : allHalls) {
            builder.addHalls(
                    DTOHall.newBuilder()
                            .setId(hall.getId())
                            .setNumber(hall.getNumber())
                            .setLayout(hall.getLayout())
                            .build()
            );
        }

        return builder.build();
    }

    public static GetHallByIdResponse createGetHallByIdResponse(Hall hall) {
        return GetHallByIdResponse.newBuilder()
                .setHall(
                        DTOHall.newBuilder()
                                .setId(hall.getId())
                                .setNumber(hall.getNumber())
                                .setLayout(hall.getLayout())
                                .build()
                )
                .build();
    }

    //---------- Movie ----------\\
    public static DTOMovie createDTOMovie(Movie movie) {
        return DTOMovie.newBuilder().
                setGenre(movie.getGenre()).
                setId(movie.getId()).
                setPlaytime(movie.getPlayTime()).
                setReleaseDate(movie.getReleaseDate().toString()).
                setTitle(movie.getTitle()).
                setDescription(movie.getDescription()).
                setPosterUrl(movie.getPoster_url()).
                setBannerUrl(movie.getBanner_url()).
                build();
    }

    public static Movie createMovie(DTOMovie dtoMovie)
    {
        return new Movie(dtoMovie.getId(), dtoMovie.getTitle(),
                dtoMovie.getPlaytime(), dtoMovie.getGenre(),
                java.time.LocalDate.parse(dtoMovie.getReleaseDate()),
                dtoMovie.getDescription(), dtoMovie.getPosterUrl(),
                dtoMovie.getBannerUrl());
    }

    public static GetAllMoviesResponse createGetAllMoviesResponse(List<Movie> movies) {
        ArrayList<DTOMovie> dtoMovies = new ArrayList<>();
        for (Movie movie : movies) {
            dtoMovies.add(createDTOMovie(movie));

        }
        return GetAllMoviesResponse.newBuilder().addAllMovies(dtoMovies).build();
    }

    public static Movie createMovie(DTOMovie dtoMovie) {
        return new Movie(dtoMovie.getId(), dtoMovie.getTitle(), dtoMovie.getPlaytime(), dtoMovie.getGenre(), LocalDate.parse( dtoMovie.getReleaseDate()));

    }


    //---------- Screening ----------\\

    public static DTOScreening createDTOScreening(Screening screening) {
        DTOMovie dtoMovie = createDTOMovie(screening.getMovie());
        DTOHall dtoHall = createDTOHall(screening.getHall());
        return DTOScreening.newBuilder().
                setId(screening.getScreeningId()).
                setHallId(screening.getHallId()).
                setStartTime(screening.getStartTime().toString()).
                setDate(screening.getDate().toString()).
                setAvailableSeats(screening.getAvailableSeats()).
                setMovie(dtoMovie).
                setHall(dtoHall).
                build();
    }

    public static Screening createScreening(DTOScreening dtoScreening) {
        Movie dtoMovie =createMovie(dtoScreening.getMovie());
        return new Screening(dtoMovie, dtoScreening.getHallId(), LocalTime.parse(dtoScreening.getStartTime()), LocalDate.parse(dtoScreening.getDate()) , dtoScreening.getAvailableSeats(), dtoScreening.getId());

    }


    public static GetAllScreeningsResponse createGetAllScreeningsResponse(
            List<Screening> screenings) {
        ArrayList<DTOScreening> dtoScreenings = new ArrayList<>();
        for (Screening s : screenings) {
            dtoScreenings.add(createDTOScreening(s));

        }
        return GetAllScreeningsResponse.newBuilder().addAllScreenings(dtoScreenings)
                .build();
    }

    public static GetAllScreeningsRequest createGetAllScreeningsRequest() {
        return GetAllScreeningsRequest.newBuilder().build();
    }

    //---------- Layout ----------\\

    public static DTOLayout createDTOLayout(Layout layout) {
        return DTOLayout.newBuilder().
                setMaxLetter(String.valueOf(layout.maxLetter)).
                setMaxSeatInt(layout.getMaxSeatInt()).build();
    }

    public static GetAllLayoutsResponse createGetAllLayoutsResponse(List<Layout> layouts) {
        ArrayList<DTOLayout> dtoLayouts = new ArrayList<>();
        for (Layout layout : layouts) {
            dtoLayouts.add(createDTOLayout(layout));
        }
        return GetAllLayoutsResponse.newBuilder().addAllLayouts(dtoLayouts).build();
    }


    public static GetAllLayoutsRequest createGetAllLayoutsRequest() {
        return GetAllLayoutsRequest.newBuilder().build();
    }


    //---------- Seat ----------\\

    public static DTOSeat createDTOSeat(Seat seat) {
        DTOSeat.Builder builder = DTOSeat.newBuilder()
                .setId(seat.getId())
                .setLetter(String.valueOf(seat.getRow()))
                .setNumber(seat.getSeatNumber())
                .setBooked(seat.isBooked());

        // Kunde KUN hvis seat er booket
        if (seat.isBooked() && seat.getCustomer() != null) {
            builder.setCustomer(createDTOCustomer(seat.getCustomer()));
        } else {
            // Sæt IKKE customer → protobuf håndterer det automatisk som null
        }

        return builder.build();

    }

    public static GetAllSeatsResponse createGetAllSeatsResponse(List<Seat> seats) {
        ArrayList<DTOSeat> dtoSeats = new ArrayList<>();
        for (Seat seat : seats) {
            dtoSeats.add(createDTOSeat(seat));
        }
        return  GetAllSeatsResponse.newBuilder().addAllSeats(dtoSeats).build();
    }

    public static GetAllSeatsRequest createGetAllSeatsRequest() {
        return GetAllSeatsRequest.newBuilder().build();
    }

}
