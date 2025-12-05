package cinema;

import cinema.model.*;
import cinema.persistence.*;

import java.time.LocalDate;
import java.time.LocalTime;
import java.util.Date;
import java.util.List;

public class main {
    public static void main(String[] args) {
        try {
            CustomerDAO dao = CustomerDAOImpl.getInstance();
            HallDAO hdao = HallDAOImpl.getInstance();
            ScreeningDAO screeningDAO = ScreeningDAOImpl.getInstance();
            SeatDAO seatDAO = SeatDAOImpl.getInstance();

            //create a new screening
//            LocalDate date =  LocalDate.of(1999, 3,31);
//            Movie movie = new Movie(1,"The Matrix", 136, "sic-fi", date );
//            Hall hall1 = new Hall(1);
//            LocalTime startTime = LocalTime.of(18,30);
//            LocalDate date1 = LocalDate.of(2025, 12, 2);
//            Screening screening1 = new Screening(movie, hall1.getId(),startTime, date1, hall1.getCapacity(), 3);
//            screeningDAO.addScreening(screening1);

            // Create a new customer
//             Customer newCustomer = new Customer("Rasmus Duus", "HelloMartin1717!", "342727@via.dk", "30439697");
//              dao.createCustomer(newCustomer);
//              System.out.println("Customer created.");

            // Retrieve the customer by email
            Customer retrieved = dao.getCustomerByEmail("342727@via.dk");
            if (retrieved != null) {
                System.out.println("Customer retrieved: " + retrieved.getName() + ", " + retrieved.getEmail());
            } else {
                System.out.println("Customer not found.");
            }
            List<Seat> seats = seatDAO.getSeatsByScreening(1);
            for (Seat seat : seats) {
                System.out.println(seat.toString());
            }


            List<Hall> hall = hdao.getAllHalls();
            for (Hall h : hall) {
                System.out.println(h.getId() + " " + h.getNumber() + " " + h.getLayout() + " " + h.getSeats().toString());
            }

            List<Screening> screenings = screeningDAO.getAllScreenings();
            for (Screening screening : screenings) {
                System.out.println(screening.toString());
            }


        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }


    }
}