using System;
using System.Collections.Generic;

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
            Console.WriteLine(_currentRoom);
            _sense.Display();
            if (_currentRoom is PitRoom)
            {
                Console.ReadKey(true);
                return;
            }
           
            if (_currentRoom is EntranceRoom && _cave.FountainRoom.IsRepaired)
            {
                Console.ReadKey(true);
                return;
            }
            _player.GetInput();
            
            if (_currentRoom is FountainRoom && _player.IsInputtingRepair())
            {
                _cave.FountainRoom.Repair();
            }
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
    
    public void Display()
    {
        SetAdjacentLocations();
        SetAdjacentRooms();
        string? currentSense = GetCurrentSense();
        string? adjacentSense;
        if (currentSense != null)
        {
            Console.WriteLine(currentSense);
        }
        
        foreach (Room room in _adjacentRooms)
        {
            adjacentSense = GetAdjacentSense(room);
            if (adjacentSense != null)
            {
                Console.WriteLine(adjacentSense);
            }
        }
    }

    private string? GetCurrentSense()
    {
        Room currentRoom = cave.GetRoomAt(player.Location);
        
        string? currentRoomSense = currentRoom switch
        {
            EntranceRoom when cave.FountainRoom.IsRepaired => "You see a light in front of you and escape the cave. You have conquered The Uncoded Ones challenge.", 
            EntranceRoom => "You see light from outside the cave. You are at the entrance.",
            FountainRoom when cave.FountainRoom.IsRepaired => "Water rushes from the Fountain of objects. It is repaired.",
            FountainRoom => "You see the silhouette of a large fountain. You are in the fountain room.",
            PitRoom => "You lose your footing and tumble into a vast chasm. You have died.",
            Room => null,
            _ => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        return currentRoomSense;
    }

    private string? GetAdjacentSense(Room currentRoom)
    {
        string? adjacentSense = currentRoom switch
        {
            EntranceRoom => null,
            FountainRoom => "You hear a faint dripping nearby. The fountain is close.",
            PitRoom => "You hear the howling of a hungry chasm. A pit is nearby.",
            Room => null,
            _ => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        return adjacentSense;
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
    private ConsoleKey GetKeyPress() => Console.ReadKey(true).Key;
    private InputActions _inputAction;
    public void GetInput()
    {
        _inputAction = GetKeyPress() switch
        {
            ConsoleKey.R => InputActions.Repair,
            ConsoleKey.W => InputActions.MoveUp,
            ConsoleKey.A => InputActions.MoveLeft,
            ConsoleKey.S => InputActions.MoveDown,
            ConsoleKey.D => InputActions.MoveRight,
            _ => InputActions.UnAccounted
        };
        
        CheckForMove();
    }

    public bool IsInputtingRepair()
    {
        if (_inputAction == InputActions.Repair) return true;

        return false;
    }
    
    private void CheckForMove()
    {
        Location? desiredLocation = new Location(Location.Row, Location.Column);
        desiredLocation = _inputAction switch
        {
            InputActions.Repair => null,
            InputActions.MoveUp when Location.Row - 1 >= 0 => Location with { Row = Location.Row - 1 },
            InputActions.MoveLeft when Location.Column - 1 >= 0 => Location with { Column = Location.Column - 1 },
            InputActions.MoveDown when Location.Row + 1 < caveRows => Location with { Row = Location.Row + 1 },
            InputActions.MoveRight when Location.Column + 1 < caveColumns => Location with { Column = Location.Column + 1 },
            _ => Location
        };

        if (desiredLocation is not null)
        {
            Location = (Location)desiredLocation;
        }
    }
    
    private enum InputActions { MoveUp, MoveDown, MoveLeft, MoveRight, Repair, UnAccounted }
}


public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    private Location _fountainLocation;
    private Location _pitLocation;

    public FountainRoom FountainRoom { get; }
    public PitRoom PitRoom { get; }
    
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
                Rooms[row, column] = new Room(new Location { Row = row, Column = column});
            }
        }

        _fountainLocation = _randomizer.GetRandomLocation();
        _pitLocation = _randomizer.GetRandomLocation();
        FountainRoom = new FountainRoom(_fountainLocation);
        PitRoom = new PitRoom(_pitLocation);
        Rooms[0, 0] = new EntranceRoom(new Location { Row = 0, Column = 0 });
        Rooms[_fountainLocation.Row, _fountainLocation.Column] = FountainRoom;
        Rooms[_pitLocation.Row, _pitLocation.Column] = PitRoom;
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
        MaxRows = _random.Next(4, 10);
        MaxColumns = _random.Next(4, 10);
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

public class EntranceRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Entrance;
}

public class PitRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Pit;
}

public class FountainRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Fountain;
    public bool IsRepaired { get; private set; }
    public void Repair()
    {
        IsRepaired = true;
    }
}

public class Room
{

    protected virtual RoomType Type { get; } = RoomType.Empty;
    public bool IsPlayerHere { get; private set; }
    public Location Location { get; }

    public Room(Location location)
    {
        Location = location;
    }
    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere;
    }
    
    protected enum RoomType { Empty, Fountain, Pit, Entrance }
}


public readonly record struct Location(int Row, int Column);