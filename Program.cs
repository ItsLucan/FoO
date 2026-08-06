using System.Data;

Cave cave = new Cave();
Player player = new Player(cave.Rows, cave.Columns);
Game game = new Game(player, cave);

game.Run();


public class Game
{
    private Player _player;
    private Cave _cave;
    private Sense _sense;
    private Room _currentRoom;

    public Game(Player player, Cave cave)
    {
        _player = player;
        _cave = cave;
        _sense = new Sense(player, cave);
        _currentRoom = _cave.GetRoomAt(_player.Location);
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            UpdatePlayerRoom();
            Display();
            Console.WriteLine(_player.Location);
            Console.WriteLine(_currentRoom.Type);
            _sense.DisplaySense();
            if (_cave.GetRoomAt(_player.Location).Type == RoomType.Pit)
            {
                Console.ReadKey(true);
                return;
            }
            _player.Move();
        }
    }

    private void UpdatePlayerRoom()
    {
        if (!_player.Location.Equals(_currentRoom.Location))
        {
            _currentRoom.SetPlayerHere(false);
            _currentRoom = _cave.GetRoomAt(_player.Location);
        }

        _currentRoom.SetPlayerHere(true);
    }
    
    private void Display()
    {
        for (int row = 0; row < _cave.Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < _cave.Rooms.GetLength(1); column++)
            {
                Console.Write(_cave.Rooms[row, column].IsPlayerHere ? "o " : "# ");
            }

            Console.WriteLine();
        }
    }
}

public class Sense(Player player, Cave cave)
{
    private List<Room> _adjacentRooms;
    private List<Location> _adjacentLocations;


    public void DisplaySense()
    {
        SetAdjacentLocations();
        SetAdjacentRooms();
        Room currentRoom = GetCurrentRoom();

        string? currentRoomSense = currentRoom.Type switch
        {
            RoomType.Empty => null,
            RoomType.Entrance => "You see light from outside the cave. You are at the entrance.",
            RoomType.Fountain => "You see the silhouette of a large fountain. You are in the fountain room.",
            RoomType.Pit => "You lose your footing and tumble into a vast chasm. You have died.",
            _ => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };
        
        if (currentRoomSense != null)
        {
            Console.WriteLine(currentRoomSense);
        }
        
        foreach (Room room in _adjacentRooms)
        {
            string? adjacentSense = room.Type switch
            {
                RoomType.Empty => null,
                RoomType.Entrance => null,
                RoomType.Fountain => "You hear a faint dripping nearby. The fountain is close.",
                RoomType.Pit => "You hear the howling of a hungry chasm. A pit is nearby.",
                _ => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
            };
            
            if (adjacentSense != null)
            {
                Console.WriteLine(adjacentSense);
            }
        }
    }

    private Room GetCurrentRoom()
    {
        return cave.GetRoomAt(player.Location);
    }
    
    private void SetAdjacentRooms()
    {
        _adjacentRooms = new List<Room>();
        
        foreach (Location location in _adjacentLocations)
        {
            _adjacentRooms.Add(cave.GetRoomAt(location));
        }
    }
    
    private void SetAdjacentLocations()
    {
        _adjacentLocations = new List<Location>();
        if (player.Location.Row - 1 >= 0)
        {
            _adjacentLocations.Add(player.Location with { Row = player.Location.Row - 1 });
        }

        if (player.Location.Row + 1 < cave.Rows)
        {
            _adjacentLocations.Add(player.Location with { Row = player.Location.Row + 1 });
        }

        if (player.Location.Column - 1 >= 0)
        {
            _adjacentLocations.Add(player.Location with { Column = player.Location.Column - 1});
        }

        if (player.Location.Column + 1 < cave.Columns)
        {
            _adjacentLocations.Add(player.Location with{ Column = player.Location.Column + 1});
        }
    }
}

public class Player(int caveRows, int caveColumns)
{
    public Location Location { get; private set; } = new Location { Row = 0, Column = 0};
    
    private ConsoleKey GetInput() => Console.ReadKey(true).Key;

    public void Move()
    {
        Location desiredLocation = new Location(Location.Row, Location.Column);
        
        desiredLocation = GetInput() switch
        {
            ConsoleKey.W when Location.Row - 1 >= 0 => Location with { Row = Location.Row - 1 },
            ConsoleKey.A when Location.Column - 1 >= 0 => Location with { Column = Location.Column - 1 },
            ConsoleKey.S when Location.Row + 1 < caveRows => Location with { Row = Location.Row + 1 },
            ConsoleKey.D when Location.Column + 1 < caveColumns => Location with { Column = Location.Column + 1 },
            _ => Location
        };

        Location = desiredLocation;
    }
}


public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    private Randomizer _randomizer = new Randomizer();
    public Cave()
    {
        Rows = _randomizer.MaxRows;
        Columns = _randomizer.MaxColumns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }

        Location randomLocation1 = _randomizer.GetRandomLocation();
        Location randomLocation2 = _randomizer.GetRandomLocation();
        Rooms[0, 0] = new Room(RoomType.Entrance, new Location { Row = 0, Column = 0 });
        Rooms[randomLocation1.Row,randomLocation1.Column] = new Room(RoomType.Fountain, randomLocation1);
        Rooms[randomLocation2.Row, randomLocation2.Column] = new Room(RoomType.Pit, randomLocation2);
    }
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}

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
        MaxRows = _random.Next(3, 10);
        MaxColumns = _random.Next(3, 10);
    }

    public Location GetRandomLocation()
    {
        _row = _random.Next(1, MaxRows);
        _column = _random.Next(1, MaxColumns);
        Location randomLocation = new Location(_row, _column);
        while (true)
        {
                if (!_randomLocations.Contains(randomLocation))
                {
                    _randomLocations.Add(randomLocation);
                    return randomLocation;
                }

                randomLocation = new Location(_random.Next(1, MaxRows), _random.Next(1, MaxColumns));
        }
    }
    
}

public class Room(RoomType type, Location location)
{
    
    public RoomType Type { get; } = type;
    public bool IsPlayerHere { get; private set; }
    public Location Location { get; } = location;

    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere;
    }
}


public readonly record struct Location(int Row, int Column);

public enum RoomType { Empty, Fountain, Pit, Entrance }