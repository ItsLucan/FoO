using System.Diagnostics;

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
    private readonly DifficultyChoice _difficulty;
    private readonly int _rows;
    private readonly int _columns;
    private readonly Location _entranceLocation;
    private readonly Location _fountainLocation;
    private readonly Location[] _pitLocations;
    private readonly Location[] _amarokLocations;
    private readonly List<Location> _staticLocations;
    private readonly Player _player;
    private readonly Cave _cave;
    private readonly List<Location> _adjacentLocations = new List<Location>();
    private readonly List<Room> _adjacentRooms = new List<Room>();
    private bool _isFountainRepaired = false;
    private bool _isGameOver = false;
    private Room _currentRoom;
    
    public Game()
    {
        int difficultyNumber;
        bool isDifficultySet = false;
        do
        {
            Console.WriteLine("\n\n\n\n\t\t\t\tChoose a difficulty:");
            Console.WriteLine("\t\t\t\t1 - Easy");
            Console.WriteLine("\t\t\t\t2 - Medium");
            Console.WriteLine("\t\t\t\t3 - Hard");
            isDifficultySet = int.TryParse(Console.ReadLine(), out difficultyNumber) && difficultyNumber is >= 1 and <= 3;
        } while (!isDifficultySet);
            
        _difficulty = difficultyNumber switch
        {
            1 => DifficultyChoice.Easy,
            2 => DifficultyChoice.Medium, 
            3 => DifficultyChoice.Hard,
        };

        if (_difficulty == DifficultyChoice.Easy)
        {
            _rows = 10;
            _columns = 10;
            _pitLocations = new Location[2];
            _amarokLocations = new Location[2];
            _staticLocations = new List<Location>();
            
            _entranceLocation = new Location(5, 0);
            _fountainLocation = new Location(2, 9);
            _pitLocations[0] = new Location(1, 3);
            _pitLocations[1] = new Location(7, 5);
            _amarokLocations[0] = new Location(8, 1);
            _amarokLocations[1] = new Location(5, 8);
            _player = new Player(_entranceLocation, _rows, _columns);
            _cave = new Cave(_rows, _columns, _entranceLocation, _fountainLocation, _pitLocations, _amarokLocations);
            _currentRoom = _cave.GetRoomAt(_entranceLocation);
            
            _staticLocations.Add(_entranceLocation);
            _staticLocations.Add(_fountainLocation);
            foreach (Location location in _pitLocations) _staticLocations.Add(location);
            foreach (Location location in _amarokLocations) _staticLocations.Add(location);
        }

        if (_difficulty == DifficultyChoice.Medium)
        {
            _rows = 15;
            _columns = 15;
            _entranceLocation = new Location(0, 7);
        }

        if (_difficulty == DifficultyChoice.Hard)
        {
            _rows = 20;
            _columns = 20;
            _entranceLocation = new Location(9, 8);
        }
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
    private void GetCurrentRoomSense()
    {
        string? currentRoomText = _currentRoom.RoomType switch
        {
            RoomType.Entrance when _isFountainRepaired  => $"{FeelColor}FEEL:{ResetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.", 
            RoomType.Fountain when _isFountainRepaired  => $"{HearColor}HEAR:{ResetColor} Water rushing from the {FountainColor}Fountain of objects{ResetColor}. It is repaired.",
            RoomType.Fountain                          => $"{SeeColor}SEE:{ResetColor} The silhouette of a large {FountainColor}fountain{ResetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{SeeColor}SEE:{ResetColor} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{NegativeColor}GAME OVER:{ResetColor} You step onto ground with no substance, and tumble into a vast chasm. You died.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };
        
        if (currentRoomText is not null) Console.WriteLine(currentRoomText); 
    }
    
    private void GetCurrentEnemySense()
    {
        string? currentEnemyText = _currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{FeelColor}FEEL:{ResetColor} The torrential strength of a {NegativeColor}Maelstrom{ResetColor}. You both are sent flying through the cave.",
            EnemyType.Amarok    => $"{NegativeColor}GAME OVER:{ResetColor} You waltz into the maw of a foul {NegativeColor}Amarok{ResetColor}, who rends your flesh. You have died.",
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
        if (_player.Location.Row + 1 < _rows) _adjacentLocations.Add(_player.Location with { Row = _player.Location.Row + 1 });
        if (_player.Location.Column - 1 >= 0) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column - 1});
        if (_player.Location.Column + 1 < _columns) _adjacentLocations.Add(_player.Location with { Column = _player.Location.Column + 1});
    }
    
    private void SetAdjacentRooms()
    {
        _adjacentRooms.Clear();
        foreach (Location location in _adjacentLocations) _adjacentRooms.Add(_cave.GetRoomAt(location));
    }   
    
    private void Display()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                
                Console.Write(_cave.Rooms[row, column].IsPlayerHere ? "o " : "# ");
            }

            Console.WriteLine();
        }
    }
}