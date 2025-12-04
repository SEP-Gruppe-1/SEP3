package cinema.dto;

import cinema.model.*;
import grpccinema.*;


import java.util.ArrayList;
import java.util.List;

public class DTOFactory {


    //---------- Customer ----------\\

    public static DTOCustomer createDTOCustomer(Customer customer) {
        return DTOCustomer.newBuilder().setName(customer.getName())
                .setPassword(customer.getPassword()).setEmail(customer.getEmail())
                .setPhone(customer.getPhone()).build();
    }

    public static Customer createCustomer(DTOCustomer dtoCustomer) {
        return new Customer(dtoCustomer.getName(), dtoCustomer.getPassword(),
                dtoCustomer.getEmail(), dtoCustomer.getPhone());
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
            dtoCustomers.add(DTOCustomer.newBuilder().setName(c.getName())
                    .setPassword(c.getPassword()).setEmail(c.getEmail())
                    .setPhone(c.getPhone()).build());
        }
        return GetCustomersResponse.newBuilder().addAllCustomers(dtoCustomers)
                .build();
    }

    //---------- Hall ----------\\

    public static DTOHall createDTOHall(Hall hall) {
        return DTOHall.newBuilder().setId(hall.getId()).setLayout(hall.getLayout())
                .setNumber(hall.getNumber()).build();
    }

    public static Hall createHall(DTOHall dtoHall) {
        return  Hall.getInstance(dtoHall.getId());
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
                build();
    }

    public static GetAllMoviesResponse createGetAllMoviesResponse(List<Movie> movies) {
        ArrayList<DTOMovie> dtoMovies = new ArrayList<>();
        for (Movie movie : movies) {
            dtoMovies.add(createDTOMovie(movie));

        }
        return GetAllMoviesResponse.newBuilder().addAllMovies(dtoMovies).build();
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


    public static GetAllScreeningsResponse createGetAllScreeningsResponse(
        List<Screening> screenings){
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


}
