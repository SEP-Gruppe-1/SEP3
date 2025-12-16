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


            seatDAO.DeleteBooking(1, "5550001");



        } catch (Exception e) {
            e.printStackTrace();
        }


    }
}