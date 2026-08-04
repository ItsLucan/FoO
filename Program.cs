Cave cave = new Cave(4, 4);
Player player = new Player();
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
            UpdatePlayerRoom();
            Display();
            Console.WriteLine(_player.Location);
            MovePlayer();
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

    private void MovePlayer()
    {
        Location desiredLocation = new Location(_player.Location.Row, _player.Location.Column);
        
        desiredLocation = _player.GetInput() switch
        {
            ConsoleKey.W when _player.Location.Row - 1 >= 0 => _player.Location with { Row = _player.Location.Row - 1 },
            ConsoleKey.A when _player.Location.Column - 1 >= 0 => _player.Location with { Column = _player.Location.Column - 1 },
            ConsoleKey.S when _player.Location.Row + 1 < _cave.Rows => _player.Location with { Row = _player.Location.Row + 1 },
            ConsoleKey.D when _player.Location.Column + 1 < _cave.Columns => _player.Location with { Column = _player.Location.Column + 1 },
            _ => _player.Location
        };
        
        _player.UpdateLocation(desiredLocation);
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


public class Player()
{
    public Location Location { get; private set; } = new Location { Row = 0, Column = 0};
    
    public ConsoleKey GetInput() => Console.ReadKey(true).Key;

    public void UpdateLocation(Location location)
    {
        Location = location;
    }
}


public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    
    public Cave(int rows, int columns)
    {
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

    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
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