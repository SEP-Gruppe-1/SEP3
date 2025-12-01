package cinema.model;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Hall {
    public int Id;
    public int Number;
    public int layout;
    public int Capacity;
    public List<Seat> Seats;

    private static final Map<Integer, Hall> instances = new HashMap<>();

    public Hall(int id) {
        this.Id = id;
        this.Number = id;
        this.layout = id;
        Seats = new ArrayList<Seat>();
        switch (layout) {
            case 2: //case for layout id= 2 == BIG_HALL
                generateSeats(8,12);
                Capacity = 96;
                break;

            case 1: // case for layout id=1 == SMALL = HALL
                generateSeats(4,10);
                Capacity = 40;
                break;
            default:
                throw new IllegalArgumentException("Invalid layout value" + layout);
        }
    }

    public static Hall getInstance(int id) {
        // Opretter kun en Hall hvis den ikke allerede findes
        instances.putIfAbsent(id, new Hall(id));
        return instances.get(id);
    }

    private void generateSeats(int rows, int seatsPerRow) {
        for (int r = 0; r < rows; r++) {
            char rowChar = (char) ('A' + r);
            for (int s = 1; s <= seatsPerRow; s++) {
                Seats.add(new Seat(rowChar, s));
            }
        }
    }


    public int getCapacity() {
        return Capacity;
    }

    public int getId() {
        return Id;
    }

    public int getLayout() {
        return layout;
    }

    public int getNumber() {
        return Number;
    }

    public List<Seat> getSeats() {
        return Seats;
    }
}
