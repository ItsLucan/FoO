namespace The_Fountain_of_Objects.Scripts;

public class GameData
{
    
    public readonly int Rows;
    public readonly int Columns;
    private readonly Random _random;
    public readonly Location EntranceLocation;
    public readonly Location FountainLocation;
    private readonly List<Location> _pitLocations;
    private readonly List<Location> _amarokLocations;
    private readonly List<Location> _maelstromLocations;
    private readonly List<Location> _hazardLocations;
    public IReadOnlyList<Location> PitLocations => _pitLocations.AsReadOnly();
    public IReadOnlyList<Location> AmarokLocations => _amarokLocations.AsReadOnly();
    public IReadOnlyList<Location> MaelstromLocations => _maelstromLocations.AsReadOnly();
    public IReadOnlyList<Location> HazardLocations => _hazardLocations.AsReadOnly(); 
  
    public GameData()
    {
        _random = new Random();
        
        int difficultyNumber;
        bool isDifficultySet = false;
        do
        {
            Console.WriteLine("Choose a difficulty:");
            Console.WriteLine("1 - Easy");
            Console.WriteLine("2 - Medium");
            Console.WriteLine("3 - Hard");
            isDifficultySet = int.TryParse(Console.ReadLine(), out difficultyNumber) && difficultyNumber is >= 1 and <= 3;
        } while (!isDifficultySet);
            
        DifficultyChoice difficulty = difficultyNumber switch
        {
            1 => DifficultyChoice.Easy,
            2 => DifficultyChoice.Medium, 
            3 => DifficultyChoice.Hard,
        }; 
        
        if (difficulty == DifficultyChoice.Easy)
        {
            Rows = 10;
            Columns = 10;
            _pitLocations = new List<Location>();
            _amarokLocations = new List<Location>();
            _maelstromLocations = new List<Location>();
            _hazardLocations = new List<Location>();
            
            EntranceLocation = new Location(5, 0);
            FountainLocation = new Location(2, 9);
            _pitLocations.Add(new Location(1, 3));
            _pitLocations.Add(new Location(7, 5));
            _amarokLocations.Add(new Location(8, 1));
            _amarokLocations.Add(new Location(5, 8));
            _maelstromLocations.Add(new Location(1, 0));
            _maelstromLocations.Add(new Location(2, 6));
            
            
            foreach (Location location in _pitLocations) _hazardLocations.Add(location);
            foreach (Location location in _amarokLocations) _hazardLocations.Add(location);
            foreach (Location location in _maelstromLocations) _hazardLocations.Add(location);
        }

        if (difficulty == DifficultyChoice.Medium)
        {
       
        }

        if (difficulty == DifficultyChoice.Hard)
        {
           
        } 
    }
  
    public Location UpdateEnemyLocations(Location maelstromLocation)
    {
            _maelstromLocations.Remove(maelstromLocation);
            _hazardLocations.Remove(maelstromLocation);
            Location newLocation = GetLocationNoHazards();
            _maelstromLocations.Add(newLocation);
            _hazardLocations.Add(newLocation);

            return newLocation;
    }
    
    //TODO doesn't really make sense here, does it? Needs to be refactored i think
    //TODO Also, maelstrom location spawning needs to be worked out. They need to not be able to spawn on eachother, but their locations are mutable
    
    public Location GetLocationNoHazards()
    {
        int row = _random.Next(0, Rows);
        int column = _random.Next(0, Columns);
        Location randomLocation = new Location(row, column);
        while (HazardLocations.Contains(randomLocation))
        {
            randomLocation = new Location(_random.Next(1, Rows), _random.Next(1, Columns));
        }
        _hazardLocations.Add(randomLocation);
        return randomLocation; 
    }
    
    public Location GetRandomLocation()
    {
        return new Location(_random.Next(0, Rows), _random.Next(0, Columns));
    } 
}