namespace The_Fountain_of_Objects.Scripts;

public static class Randomizer
{
    private static readonly Random Random;
    public static readonly int Rows;
    public static readonly int Columns;
    private static readonly List<Location> RandomLocations;
    
    static Randomizer()
    {
        Random = new Random();
        RandomLocations = new List<Location>();
        Rows = Random.Next(10, 20);
        Columns = Random.Next(10, 20);
    }
        
    public static Location GetSafeRandomLocation()
    {
        int row = Random.Next(2, Rows);
        int column = Random.Next(2, Columns);
        Location randomLocation = new Location(row, column);
        while (RandomLocations.Contains(randomLocation)) randomLocation = new Location(Random.Next(1, Rows), Random.Next(1, Columns));
        RandomLocations.Add(randomLocation);
        return randomLocation; 
    }
    
    public static Location GetRandomLocation()
    {
        return new Location(Random.Next(0, Rows), Random.Next(0, Columns));
    } 
}