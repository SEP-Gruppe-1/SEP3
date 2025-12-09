namespace Entities;

public class Layout
{
    public static readonly Dictionary<int, Layout> instance = new();


    private Layout(char maxLetter, int maxSeatInt, int id)
    {
        this.id = id;
        this.maxSeatInt = maxSeatInt;
        this.maxLetter = maxLetter;
    }

    public char maxLetter { get; set; }
    public int maxSeatInt { get; set; }
    public int id { get; set; }

    public static Layout Create(int id, char maxLetter, int maxSeatInt)
    {
        if (!instance.ContainsKey(id)) instance[id] = new Layout(maxLetter, maxSeatInt, id);

        return instance[id];
    }

    public static Layout GetInstance(int id)
    {
        instance.TryGetValue(id, out var layout);
        return layout;
    }
}