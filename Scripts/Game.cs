namespace The_Fountain_of_Objects.Scripts;

public class Game
{
    private const string SeeColor      = "\e[38;2;251;245;43m";
    private const string HearColor     = "\e[38;2;137;251;43m";
    private const string FeelColor     = "\e[38;2;194;49;160m";
    private const string SmellColor    = "\e[38;2;251;159;43m";
    private const string NegativeColor = "\e[38;2;172;48;0m";
    private const string FountainColor = "\e[38;2;60;120;255m";
    private const string ResetColor    = "\e[39m";
    private readonly Player _player;
    private readonly Cave _cave;
    private readonly List<Location> _adjacentLocations = new List<Location>();
    private readonly List<Room> _adjacentRooms = new List<Room>();
    private bool _isFountainRepaired;
    private bool _isGameOver;
    private Room _currentRoom;
    
    public Game()
    {
        GameData gameData = new GameData();
        _player = new Player(gameData);
        _cave = new Cave(gameData);
        _currentRoom = _cave.EntranceRoom;
    }
    
    public void Run()
    {
        Console.Write("You have entered the cave holding the fountain of objects. Can you survive?");
        DisplayMenuOnInput(); 
        
        while (true)
        {
            Console.Clear();
            UpdatePlayerRoom();
            SetAdjacentLocations();
            SetAdjacentRooms(); 
            Display();
            GetCurrentRoomSense();
            GetCurrentEnemySense();
           
            _isGameOver = CheckIsGameOver();
            if (_isGameOver)
            {
                Console.ReadKey(true);
                return;
            }
            
            foreach (Room room in _adjacentRooms)
            {
                GetAdjacentRoomSense(room); 
                GetAdjacentEnemySense(room);
            }
            
            if (_currentRoom.EnemyType == EnemyType.Maelstrom)
            {
                _cave.MoveMaelstrom(_player.Location);
                _player.Teleport();
            }
            
            _player.GetInput();
            if (_player.ArrowLocation is not null)
            {
                Location copyLocation = (Location)_player.ArrowLocation;
                Room targetLocation = _cave.GetRoomAt(copyLocation);
                if (targetLocation.EnemyType != EnemyType.None) targetLocation.SetEnemyHere(EnemyType.None);
            }
            
            RepairOnInput();
            DisplayMenuOnInput();
        }
    }
    
    
    private void DisplayMenuOnInput()
    {
        if (!_player.IsOpeningMenu()) return;
        
            Console.Clear();
            Console.WriteLine("MOVEMENT: WASD or Arrow keys\n");
            //TODO need to have "shooting mode" exitable
            Console.WriteLine("SHOOTING: Press spacebar to enter shoot mode.\n");
            Console.WriteLine("REPAIRING: R Key\n");
            Console.WriteLine("To open this menu, press TAB\n");
            Console.WriteLine("Press any key to exit menu.");
            Console.ReadKey(true);     
    }

    private void RepairOnInput()
    {
        if (_currentRoom.RoomType is not RoomType.Fountain || !_player.IsInputtingRepair()) return;
        if (!_isFountainRepaired) _isFountainRepaired = true;
    }
    
    private bool CheckIsGameOver()
    {
        return _currentRoom.RoomType is RoomType.Pit
               || _currentRoom.RoomType is RoomType.Entrance && _isFountainRepaired
               || _currentRoom.EnemyType is EnemyType.Amarok;
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
    private void GetCurrentRoomSense()
    {
        string? currentRoomText = _currentRoom.RoomType switch
        {
            RoomType.Entrance when _isFountainRepaired  => $"{FeelColor}FEEL:{ResetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.\n\nPress any key to quit.", 
            RoomType.Fountain when _isFountainRepaired  => $"{HearColor}HEAR:{ResetColor} Water rushing from the {FountainColor}Fountain of objects{ResetColor}. It is repaired.",
            RoomType.Fountain                          => $"{SeeColor}SEE:{ResetColor} The silhouette of a large {FountainColor}fountain{ResetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{SeeColor}SEE:{ResetColor} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{NegativeColor}GAME OVER:{ResetColor} You step onto ground with no substance, and tumble into a vast chasm.\n\nPress any key to quit.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };
        
        if (currentRoomText is not null) Console.WriteLine(currentRoomText); 
    }
    
    private void GetCurrentEnemySense()
    {
        string? currentEnemyText = _currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{FeelColor}FEEL:{ResetColor} The torrential strength of a {NegativeColor}Maelstrom{ResetColor}. You both are sent flying through the cave.\n\nPress any key to continue.",
            EnemyType.Amarok    => $"{NegativeColor}GAME OVER:{ResetColor} You waltz into the maw of a foul {NegativeColor}Amarok{ResetColor}, who rends your flesh. You have died.\n\nPress any key to quit.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        if (currentEnemyText is not null) Console.WriteLine(currentEnemyText);
    }

    private void GetAdjacentRoomSense(Room adjacentRoom)
    {
        string? adjacentRoomText = adjacentRoom.RoomType switch
        {
            RoomType.Fountain                   => $"{HearColor}HEAR:{ResetColor} A faint dripping in the distance. The {FountainColor}fountain{ResetColor} is close.",
            RoomType.Pit                        => $"{FeelColor}FEEL:{ResetColor} The howling breath of a hungry chasm. A {NegativeColor}pit{ResetColor} is nearby.",
            RoomType.Entrance or RoomType.Empty => null,    
            _                                   => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };
            
        if (adjacentRoomText is not null) Console.WriteLine(adjacentRoomText);
    }

    private void GetAdjacentEnemySense(Room adjacentRoom)
    {
        string? adjacentEnemyText = adjacentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{FeelColor}FEEL:{ResetColor} The ghastly winds of a {NegativeColor}Maelstrom{ResetColor} nearby.",
            EnemyType.Amarok    => $"{SmellColor}SMELL:{ResetColor} The pungent odor of rotten flesh. An {NegativeColor}Amarok{ResetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        }; 
        
        if (adjacentEnemyText is not null) Console.WriteLine(adjacentEnemyText);
    }
    
    private void SetAdjacentLocations()
    {
        _adjacentLocations.Clear();
        if (_player.Location.Row - 1 >= 0) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row - 1 });
        if (_player.Location.Row + 1 < _cave.Rows) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row + 1 });
        if (_player.Location.Column - 1 >= 0) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column - 1});
        if (_player.Location.Column + 1 < _cave.Columns) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column + 1});
    }
    
    private void SetAdjacentRooms()
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
                if (_player.ArrowLocation is not null && _cave.Rooms[row, column].Location == _player.ArrowLocation)
                {
                    Console.Write("❌ ");
                }
                else if (_cave.Rooms[row, column].IsPlayerHere)
                {
                    if (_cave.Rooms[row, column].RoomType == RoomType.Pit ||
                        _cave.Rooms[row, column].EnemyType == EnemyType.Amarok)
                    {
                        Console.Write("😵 ");
                    }
                    else Console.Write($"😊 ");
                }
                else if (_cave.Rooms[row, column].RoomType == RoomType.Entrance)
                {
                    Console.Write("🚪 ");
                }
                else if (_cave.Rooms[row, column].HasPlayerVisited)
                {
                    if (_cave.Rooms[row, column].RoomType == RoomType.Fountain) Console.Write("⛲ ");
                    else Console.Write("   ");
                }
                else
                {
                    Console.Write("## ");
                }
            }

            Console.WriteLine();
        }
    }
}