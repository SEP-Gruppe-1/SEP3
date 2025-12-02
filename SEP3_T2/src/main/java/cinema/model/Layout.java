package cinema.model;

import java.util.HashMap;
import java.util.Map;

public class Layout {

    public char maxLetter;
    public int maxSeatInt;
    public int id;

    private static final Map<Integer,Layout> instance = new HashMap<Integer,Layout>();

    private Layout(char maxLetter, int maxSeatInt, int id) {
        this.id=id;
        this.maxSeatInt=maxSeatInt;
        this.maxLetter=maxLetter;
    }

    public static Layout create(int id, char maxLetter, int maxSeatInt) {
        return instance.computeIfAbsent(id,
                k -> new Layout(maxLetter, maxSeatInt, id)
        );
    }

    public static Layout getInstance(int id){

        return instance.get(id);
    }

    public char getMaxLetter() {
        return maxLetter;
    }

    public int getMaxSeatInt() {
        return maxSeatInt;
    }

    public void setMaxLetter(char maxLetter) {
        this.maxLetter = maxLetter;
    }

    public void setMaxSeatInt(int maxSeatInt) {
        this.maxSeatInt = maxSeatInt;
    }

}
