namespace The_Fountain_of_Objects.Scripts;


public class Game
{
    private readonly Player _player;
    private readonly Cave _cave;
    private readonly Sensor _sensor;
    private readonly List<Location> _adjacentLocations = [];
    private readonly List<Room> _adjacentRooms = [];
    private bool _isFountainRepaired;
    private bool _hasPlayerLost;
    private bool _hasPlayerWon;
    private Room _currentRoom;
   
    
    public Game()
    {
        GameData gameData = new(); 
        _player = new Player(gameData);
        _cave = new Cave(gameData);
        _sensor = new Sensor();
        _currentRoom = _cave.GetRoomAt(gameData.EntranceLocation);
    }
   
    
    public void Run()
    {
        DisplayMenu(); 
        
        while (true)
        {
            Console.Clear();
            UpdatePlayerRoom();
            UpdateAdjacentLocations();
            UpdateAdjacentRooms();
            _hasPlayerLost = _currentRoom.RoomType is RoomType.Pit || _currentRoom.EnemyType is EnemyType.Amarok;
            _hasPlayerWon = _currentRoom.RoomType is RoomType.Entrance && _isFountainRepaired;
            Display();
            
            if (_hasPlayerLost || _hasPlayerWon) break;
            
            foreach (Room room in _adjacentRooms) _sensor.DisplayAdjacentSenses(room); 
            
            if (_currentRoom.EnemyType == EnemyType.Maelstrom)
            {
                _cave.TeleportMaelstromAt(_player.Location);
                _player.Teleport();
            }
            
            _player.ProcessInput();
            ShootOnInput();
            RepairOnInput();
            if (_player.IsOpeningMenu()) DisplayMenu();
        }

        Console.ReadKey(true);
    }
    
    
    private void DisplayMenu()
    {
            Console.Clear();
            Console.WriteLine("MOVEMENT: WASD or Arrow keys\n");
            Console.WriteLine("SHOOTING: Spacebar to draw an arrow, press again to confirm.");
            Console.WriteLine("Esc (Not in pseudoterminal) or E to cancel.\n");
            Console.WriteLine("REPAIRING: R Key\n");
            Console.WriteLine("To open this menu, press TAB\n");
            Console.WriteLine("Press any key to exit menu.");
            Console.ReadKey(true);     
    }

    
    private void RepairOnInput()
    {
        if (_currentRoom.RoomType != RoomType.Fountain || !_player.IsInputtingRepair()) return;
        if (!_isFountainRepaired) _isFountainRepaired = true;
    }


    private void ShootOnInput()
    {
        if (!_player.IsInputtingShoot() || _player.Arrows <= 0) return;
        _player.SetArrowHere();
        
        do
        {
            Console.Clear();
            Display();
            foreach (Room room in _adjacentRooms) _sensor.DisplayAdjacentSenses(room);
            _player.ProcessArrowInput();
        } 
        while (!_player.IsInputtingShoot() && !_player.IsInputtingExit());

        if (!_player.IsInputtingExit() && _player.ArrowLocation is not null)
        {
            _player.SubtractArrow();
            Room targetRoom = _cave.GetRoomAt((Location)_player.ArrowLocation);
            if (targetRoom.EnemyType != EnemyType.None) targetRoom.SetEnemyHere(EnemyType.None);
        }
                
        _player.ResetArrow();
    }
    
    
    private void UpdatePlayerRoom()
    {
        if (_currentRoom != _cave.GetRoomAt(_player.Location))
        {
            _currentRoom.SetPlayerHere(false);
            _currentRoom = _cave.GetRoomAt(_player.Location);
        }

        _currentRoom.SetPlayerHere(true);
    }
    
    
    private void Display()
    {
        for (int row = 0; row < _cave.Rows; row++)
        {
            for (int column = 0; column < _cave.Columns; column++)
            {
                Location indexLocation = new Location(row, column);
                
                if      (indexLocation == _player.ArrowLocation && _player.ArrowLocation is not null) Console.Write("🏹 ");
                else if (_cave.GetRoomAt(indexLocation).IsPlayerHere) Console.Write(_hasPlayerLost ? "😵 " : _hasPlayerWon ? $"🥳 " : "😊 ");
                else if (_cave.GetRoomAt(indexLocation).RoomType == RoomType.Entrance) Console.Write("🚪 ");
                else if (_cave.GetRoomAt(indexLocation).HasPlayerVisited) Console.Write(_cave.GetRoomAt(indexLocation).RoomType == RoomType.Fountain ? "⛲ " : "   ");
                else Console.Write("## ");
            }

            Console.WriteLine();
        }
        
        Console.WriteLine("-------------------");
        Console.WriteLine($"Current Arrows: {_player.Arrows}");
        Console.WriteLine("-------------------");
        _sensor.DisplayCurrentSenses(_currentRoom, _isFountainRepaired);
    }
    
    
    private void UpdateAdjacentLocations()
    {
        _adjacentLocations.Clear();
        if (_player.Location.Row - 1 >= 0) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row - 1 });
        if (_player.Location.Row + 1 < _cave.Rows) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row + 1 });
        if (_player.Location.Column - 1 >= 0) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column - 1});
        if (_player.Location.Column + 1 < _cave.Columns) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column + 1});
    }
   
    
    private void UpdateAdjacentRooms()
    {
        _adjacentRooms.Clear();
        foreach (Location location in _adjacentLocations) _adjacentRooms.Add(_cave.GetRoomAt(location));
    }   
}