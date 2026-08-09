namespace The_Fountain_of_Objects.Scripts;

public static class Randomizer
{
    private static readonly Random Random;
    public static readonly int MaxRows;
    public static readonly int MaxColumns;
    private static readonly List<Location> RandomLocations;
    
    static Randomizer()
    {
        Random = new Random();
        RandomLocations = new List<Location>();
        MaxRows = Random.Next(5, 10);
        MaxColumns = Random.Next(5, 10);
    }
        
    public static Location GetSafeRandomLocation()
    {
        int row = Random.Next(2, MaxRows);
        int column = Random.Next(2, MaxColumns);
        Location randomLocation = new Location(row, column);
        while (RandomLocations.Contains(randomLocation)) randomLocation = new Location(Random.Next(1, MaxRows), Random.Next(1, MaxColumns));
        RandomLocations.Add(randomLocation);
        return randomLocation; 
    }
    
    public static Location GetRandomLocation()
    {
        return new Location(Random.Next(0, MaxRows), Random.Next(0, MaxColumns));
    } 
}