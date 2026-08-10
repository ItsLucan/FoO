namespace The_Fountain_of_Objects.Scripts;

public class Game
{
    private Player _player;
    private Cave _cave;
    private Sensor _sensor;
    private Room _currentRoom;
    private List<Location> _adjacentLocations = new List<Location>();
    private List<Room> _adjacentRooms = new List<Room>();
    private bool _isFountainRepaired = false;
    private bool _isGameOver = false;
    public Game(Player player, Cave cave)
    {
        _player = player;
        _cave = cave;
        _currentRoom = _cave.GetRoomAt(_player.Location);
        _sensor = new Sensor();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            UpdatePlayerRoom();
            SetAdjacentLocations();
            SetAdjacentRooms(); 
            Display();
            _sensor.GetSenses(_currentRoom, _adjacentRooms, _isFountainRepaired);
            _isGameOver = CheckIsGameOver();
            if (_isGameOver)
            {
                Console.ReadKey(true);
                return;
            }

            if (_currentRoom.EnemyType == EnemyType.Maelstrom)
            {
                _player.Teleport(Randomizer.GetRandomLocation());
                _cave.MoveMaelstrom();
            }
            
            _player.GetInput();
            
            if (_currentRoom.RoomType is RoomType.Fountain && _player.IsInputtingRepair())
            {
                _isFountainRepaired = true;
            }
        }
    }

    private bool CheckIsGameOver()
    {
        if (_currentRoom.RoomType is RoomType.Pit
            || _currentRoom.RoomType is RoomType.Entrance && _isFountainRepaired
            || _currentRoom.EnemyType is EnemyType.Amarok)
        {
            return true;
        }

        return false;
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
    
    private void SetAdjacentLocations()
    {
        _adjacentLocations.Clear();
        if (_player.Location.Row - 1 >= 0) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row - 1 });
        if (_player.Location.Row + 1 < Randomizer.MaxRows) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row + 1 });
        if (_player.Location.Column - 1 >= 0) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column - 1});
        if (_player.Location.Column + 1 < Randomizer.MaxColumns) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column + 1});
    }
    
    private void SetAdjacentRooms()
    {
        _adjacentRooms.Clear();
        foreach (Location location in _adjacentLocations) _adjacentRooms.Add(_cave.GetRoomAt(location));
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