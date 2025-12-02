package cinema.model;

public class Layout {

    public char maxLetter;
    public int maxInt;

    public Layout(char maxLetter, int maxInt) {
        this.maxInt=maxInt;
        this.maxLetter=maxLetter;
    }

    public char getMaxLetter() {
        return maxLetter;
    }

    public int getMaxInt() {
        return maxInt;
    }

    public void setMaxLetter(char maxLetter) {
        this.maxLetter = maxLetter;
    }

    public void setMaxInt(int maxInt) {
        this.maxInt = maxInt;
    }

}
