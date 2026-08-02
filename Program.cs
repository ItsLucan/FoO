Player player = new Player();
Cave cave = new Cave(4, 4);
Game game = new Game(player, cave);

game.Run();

public class Game
{
    private Player _player;
    private Cave _cave;
    private Room _currentRoom;

    public Game(Player player, Cave cave)
    {
        _player = player;
        _cave = cave;
        _currentRoom = _cave.GetRoomAt(_player.Location);
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            UpdatePlayerPosition();
            Display();
            Console.WriteLine(_player.Location);
            _player.Move();
        }
    }

    private void UpdatePlayerPosition()
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

public class Player
{
    public Location Location { get; private set; } = new Location { Row = 0, Column = 0};
    // TODO find a way to remove magic numbers from movement switch
    public void Move()
    {
        ConsoleKey input = Console.ReadKey(true).Key;
        Location = input switch
        { 
            ConsoleKey.W when Location.Row - 1 >= 0 => Location with { Row = Location.Row - 1 },
            ConsoleKey.A when Location.Column - 1 >= 0 => Location with { Column = Location.Column - 1 },
            ConsoleKey.S when Location.Row + 1 < 4 => Location with { Row = Location.Row + 1 },
            ConsoleKey.D when Location.Column + 1 < 4 => Location with { Column = Location.Column + 1 },
            _ => Location
        };
    }
}

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    
    public Cave(int rows, int columns)
    {
        // TODO does this make sense to have Row/Column properties so player knows movelimit bounds? Should player know that?
        Rows = rows;
        Columns = columns;
        
        Rooms = new Room[Rows, Columns];
        
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }

        // TODO possible implementation of different Rooms? Rooms[0, 0] = new Room(RoomType.Entrance, new Location(0, 0));
    }

    public Room GetRoomAt(Location location)
    {
        return Rooms[location.Row, location.Column];
    }
}

public class Room(RoomType type, Location location)
{
    
    public RoomType Type { get; private set; } = type;
    public bool IsPlayerHere { get; private set; } = false;
    public Location Location { get; private set; } = location;

    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere;
    }
}

public readonly record struct Location(int Row, int Column);

public enum RoomType { Empty, Fountain, Pit, Entrance }