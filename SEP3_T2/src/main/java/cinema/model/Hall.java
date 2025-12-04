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
    public Layout layouts;

    private static final Map<Integer, Hall> instances = new HashMap<>();

    private Hall(int id) {
        this.Id = id;
        this.Number = id;
        this.layout = id;
        Seats = new ArrayList<Seat>();
        this.layouts = Layout.getInstance(id);
        int maxletter= layouts.getMaxLetter() -'A' +1;
        Capacity = maxletter* layouts.getMaxSeatInt();
        generateSeats(maxletter,layouts.getMaxSeatInt());
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

    @Override
    public String toString() {
        return " denne hall har nummer" + getNumber();
    }
}
