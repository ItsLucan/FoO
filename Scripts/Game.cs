namespace The_Fountain_of_Objects.Scripts;

public class Game
{
    private Player _player;
    private Cave _cave;
    private Sensor _sensor;
    private Room _currentRoom;
    public bool IsFountainRepaired { get; private set; } = false;
    public Game(Player player, Cave cave)
    {
        _player = player;
        _cave = cave;
        _sensor = new Sensor(player, cave);
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
            _sensor.GetSenses();
            if (_currentRoom.RoomType is RoomType.Pit
                || _currentRoom.RoomType is RoomType.Entrance && IsFountainRepaired
                || _currentRoom.EnemyType is EnemyType.Amarok)
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
                IsFountainRepaired = true;
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