package cinema.model;

public class Seat {
    public char row;
    public int seatNumber;
    public int id ;
    public boolean booked;
    public Customer customer;

    public Seat(char row, int seatNumber, int id) {
        this.row = row;
        this.seatNumber = seatNumber;
        this.id = id;
        this.booked = false;
        this.customer = null;
    }

    public char getRow() {
        return row;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getSeatNumber() {
        return seatNumber;
    }

    public void bookSeat(Customer customer) {
        this.booked = true;
        this.customer = customer;

    }
    public void unBookSeat() {
        this.booked = false;
        this.customer = null;
    }

    public boolean isBooked() {
        return booked;
    }

    @Override
    public String toString() {
        if (customer != null) {
            return "Seat{" + "row=" + row + ", seatNumber=" + seatNumber + '}' + " " + isBooked() + " " + customer.getPhone();
        }
        return "Seat{" + "row=" + row + ", seatNumber=" + seatNumber + '}' + " " + isBooked();
    }
}
