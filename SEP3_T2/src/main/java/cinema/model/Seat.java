package cinema.model;

public class Seat {
    public char row;
    public int seatNumber;

    public Seat(char row, int seatNumber) {
        this.row = row;
        this.seatNumber = seatNumber;
    }

    public char getRow() {
        return row;
    }

    public int getSeatNumber() {
        return seatNumber;
    }
    @Override
    public String toString() {
        return  "Seat{" + "row=" + row + ", seatNumber=" + seatNumber + '}';
    }
}
