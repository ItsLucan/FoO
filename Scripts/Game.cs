namespace The_Fountain_of_Objects.Scripts;

public class Game
{
    private const string See           = $"\e[38;2;251;245;43mSEE:{ResetColor}";
    private const string Hear          = $"\e[38;2;137;251;43mHEAR:{ResetColor}";
    private const string Feel          = $"\e[38;2;194;49;160mFEEL:{ResetColor}";
    private const string Smell         = $"\e[38;2;251;159;43mSMELL:{ResetColor}";
    private const string NegativeColor = "\e[38;2;172;48;0m";
    private const string FountainColor = "\e[38;2;60;120;255m";
    private const string ResetColor    = "\e[39m";
    private readonly Player _player;
    private readonly Cave _cave;
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
            DisplayCurrentRoomSense();
            DisplayCurrentEnemySense();
            
            if (_hasPlayerLost || _hasPlayerWon) break;
            
            foreach (Room room in _adjacentRooms)
            {
                DisplayAdjacentRoomSenseFor(room); 
                DisplayAdjacentEnemySenseFor(room);
            }
            
            if (_currentRoom.EnemyType == EnemyType.Maelstrom)
            {
                _cave.TeleportMaelstromAt(_player.Location);
                _player.Teleport();
            }
            
            _player.ProcessInput();
            
            if (_player.IsInputtingShoot() && _player.Arrows > 0)
            {
                _player.SetArrowHere();
                
                do
                {
                    Console.Clear();
                    Display();
                    _player.TryMoveArrow();
                } while (!_player.IsInputtingShoot() && !_player.IsInputtingEscape());

                if (!_player.IsInputtingEscape() && _player.ArrowLocation is not null)
                {
                    _player.SubtractArrow();
                    Room targetRoom = _cave.GetRoomAt((Location)_player.ArrowLocation);
                    if (targetRoom.EnemyType != EnemyType.None) targetRoom.SetEnemyHere(EnemyType.None);
                }
                
                _player.ResetArrow();
            }
            
            RepairOnInput();
            if (_player.IsOpeningMenu()) DisplayMenu();
        }

        Console.ReadKey(true);
    }
    
    
    private void DisplayMenu()
    {
            Console.Clear();
            Console.WriteLine("MOVEMENT: WASD or Arrow keys\n");
            //TODO need to have "shooting mode" exit-able
            Console.WriteLine("SHOOTING: Spacebar to draw an arrow, press again to confirm. Esc to cancel.\n");
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
    

    private void UpdatePlayerRoom()
    {
        if (_currentRoom.Location != _player.Location)
        {
            _currentRoom.SetPlayerHere(false);
            _currentRoom = _cave.GetRoomAt(_player.Location);
        }

        _currentRoom.SetPlayerHere(true);
    }
    
    private void DisplayCurrentRoomSense()
    {
        string? currentRoomText = _currentRoom.RoomType switch
        {
            RoomType.Entrance when _isFountainRepaired  => $"{Feel} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.\n\nPress any key to quit.", 
            RoomType.Fountain when _isFountainRepaired  => $"{Hear} Water rushing from the {FountainColor}Fountain of objects{ResetColor}. It is repaired.",
            RoomType.Fountain                          => $"{See} The silhouette of a large {FountainColor}fountain{ResetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{See} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{NegativeColor}GAME OVER:{ResetColor} You step onto ground with no substance, and tumble into a vast chasm.\n\nPress any key to quit.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };
        
        if (currentRoomText is not null) Console.WriteLine(currentRoomText); 
    }
    
    private void DisplayCurrentEnemySense()
    {
        string? currentEnemyText = _currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{Feel} The torrential strength of a {NegativeColor}Maelstrom{ResetColor}. You both are sent flying through the cave.\n\nPress any key to continue.",
            EnemyType.Amarok    => $"{NegativeColor}GAME OVER:{ResetColor} You waltz into the maw of a foul {NegativeColor}Amarok{ResetColor}, who rends your flesh. You have died.\n\nPress any key to quit.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        if (currentEnemyText is not null) Console.WriteLine(currentEnemyText);
    }

    private void DisplayAdjacentRoomSenseFor(Room adjacentRoom)
    {
        string? adjacentRoomText = adjacentRoom.RoomType switch
        {
            RoomType.Fountain                   => $"{Hear} A faint dripping in the distance. The {FountainColor}fountain{ResetColor} is close.",
            RoomType.Pit                        => $"{Feel} The howling breath of a hungry chasm. A {NegativeColor}pit{ResetColor} is nearby.",
            RoomType.Entrance or RoomType.Empty => null,    
            _                                   => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };
            
        if (adjacentRoomText is not null) Console.WriteLine(adjacentRoomText);
    }

    private void DisplayAdjacentEnemySenseFor(Room adjacentRoom)
    {
        string? adjacentEnemyText = adjacentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{Feel} The ghastly winds of a {NegativeColor}Maelstrom{ResetColor} nearby.",
            EnemyType.Amarok    => $"{Smell} The pungent odor of rotten flesh. An {NegativeColor}Amarok{ResetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        }; 
        
        if (adjacentEnemyText is not null) Console.WriteLine(adjacentEnemyText);
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
    
    private void Display()
    {
        for (int row = 0; row < _cave.Rows; row++)
        {
            for (int column = 0; column < _cave.Columns; column++)
            {
                if (_player.ArrowLocation is not null && _cave.Rooms[row, column].Location == _player.ArrowLocation) Console.Write("❌ ");
                else if (_cave.Rooms[row, column].IsPlayerHere) Console.Write(_hasPlayerLost ? "😵 " : _hasPlayerWon ? $"🥳 " : "😊 ");
                else if (_cave.Rooms[row, column].RoomType == RoomType.Entrance) Console.Write("🚪 ");
                else if (_cave.Rooms[row, column].HasPlayerVisited) Console.Write(_cave.Rooms[row, column].RoomType == RoomType.Fountain ? "⛲ " : "   ");
                else Console.Write("## ");
            }

            Console.WriteLine();
        }
        
        Console.WriteLine("-------------------");
        Console.WriteLine($"Current Arrows: {_player.Arrows}");
        Console.WriteLine("-------------------"); 
    }
}