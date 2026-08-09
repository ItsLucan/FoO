namespace The_Fountain_of_Objects;

public class Randomizer
{
    private Random _random = new Random();
    public  int MaxRows { get; }
    public int MaxColumns { get; } 
    private int _row;
    private int _column;
    private List<Location> _randomLocations = new List<Location>();
    public Randomizer()
    {
        MaxRows = _random.Next(5, 10);
        MaxColumns = _random.Next(5, 10);
    }

    public Location GetRandomLocationNoOverlap()
    {
        _row = _random.Next(2, MaxRows);
        _column = _random.Next(2, MaxColumns);
        Location randomLocation = new Location(_row, _column);
        while (_randomLocations.Contains(randomLocation)) randomLocation = new Location(_random.Next(1, MaxRows), _random.Next(1, MaxColumns));
        _randomLocations.Add(randomLocation);
        return randomLocation;
    }

    public Location GetRandomLocation()
    {
        _row = _random.Next(0, MaxRows);
        _column = _random.Next(0, MaxColumns);
        return new Location(_row, _column);
    }
}