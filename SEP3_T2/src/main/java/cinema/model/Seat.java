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

    public void bookSeat() {
        this.booked = true;

    }
    public void unBookSeat() {
        this.booked = false;
    }


    @Override
    public String toString() {
        return  "Seat{" + "row=" + row + ", seatNumber=" + seatNumber + '}';
    }
}
