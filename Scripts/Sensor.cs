namespace The_Fountain_of_Objects.Scripts;

public class Sensor(Player player, Cave cave)
{
    private string _seeColor      = "\e[38;2;251;245;43m";
    private string _hearColor     = "\e[38;2;137;251;43m";
    private string _feelColor     = "\e[38;2;194;49;160m";
    private string _smellColor    = "\e[38;2;251;159;43m";
    private string _negativeColor = "\e[38;2;172;48;0m";
    private string _fountainColor = "\e[38;2;60;120;255m";
    private string _resetColor    = "\e[39m";
    private List<Room> _adjacentRooms;
    private List<Location> _adjacentLocations;
    
    public void GetSenses()
    {
        SetAdjacentLocations();
        SetAdjacentRooms();
        (string? currentRoomSense, string? currentEnemySense) = GetCurrentSenses();
        if (currentRoomSense != null) Console.WriteLine(currentRoomSense);
        if (currentEnemySense != null) Console.WriteLine(currentEnemySense);
        
        foreach (Room room in _adjacentRooms)
        {
            (string? adjacentRoomSense, string? adjacentEnemySense) = GetAdjacentSenses(room);
            if (adjacentRoomSense != null) Console.WriteLine(adjacentRoomSense);
            if (adjacentEnemySense != null) Console.WriteLine(adjacentEnemySense);
        }
    }

    private (string? currentRoom, string? currentEnemy) GetCurrentSenses()
    {
        Room currentRoom = cave.GetRoomAt(player.Location);
        
        string? currentRoomSense = currentRoom.RoomType switch
        {
            RoomType.Entrance when                              => $"{_feelColor}FEEL:{_resetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.", 
            RoomType.Fountain when                              => $"{_hearColor}HEAR:{_resetColor} Water rushing from the {_fountainColor}Fountain of objects{_resetColor}. It is repaired.",
            RoomType.Fountain                                   => $"{_seeColor}SEE:{_resetColor} The silhouette of a large {_fountainColor}fountain{_resetColor}. You are in the fountain room.",
            RoomType.Entrance                                   => $"{_seeColor}SEE:{_resetColor} light from outside the cave. You are at the entrance.",
            RoomType.Pit                                        => $"{_negativeColor}GAME OVER:{_resetColor} You step onto ground with no substance, and tumble into a vast chasm. You died.",
            RoomType.Empty                                      => null,
            _                                                   => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        string? currentEnemySense = currentRoom.EnemyType switch
        {
            
            EnemyType.Maelstrom => $"{_feelColor}FEEL:{_resetColor} The torrential strength of a Maelstrom. You both are sent flying through the cave.",
            EnemyType.Amarok    => $"{_negativeColor}GAME OVER:{_resetColor} You waltz into the maw of a foul Amarok, who rends your flesh. You have died.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        return (currentRoomSense, currentEnemySense);
    }

    private (string? adjacentRoom, string? adjacentEnemy) GetAdjacentSenses(Room currentRoom)
    {
        string? adjacentRoom = currentRoom.RoomType switch
        {
            RoomType.Fountain                   => $"{_hearColor}HEAR:{_resetColor} A faint dripping in the distance. The {_fountainColor}fountain{_resetColor} is close.",
            RoomType.Pit                        => $"{_feelColor}FEEL:{_resetColor} The howling breath of a hungry chasm. A {_negativeColor}pit{_resetColor} is nearby.",
            RoomType.Entrance or RoomType.Empty => null,    
            _                                   => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        string? adjacentEnemy = currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{_feelColor}FEEL:{_resetColor} The ghastly winds of a {_negativeColor}Maelstrom{_resetColor} nearby.",
            EnemyType.Amarok    => $"{_smellColor}SMELL:{_resetColor} The pungent odor of rotten flesh. An {_negativeColor}Amarok{_resetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        };
        return (adjacentRoom, adjacentEnemy);
    }
    
    private void SetAdjacentRooms()
    {
        _adjacentRooms = new List<Room>();
        
        foreach (Location location in _adjacentLocations) _adjacentRooms.Add(cave.GetRoomAt(location));
    }
    
    private void SetAdjacentLocations()
    {
        _adjacentLocations = new List<Location>();
        if (player.Location.Row - 1 >= 0) _adjacentLocations.Add(player.Location with { Row = player.Location.Row - 1 });
        if (player.Location.Row + 1 < cave.Rows) _adjacentLocations.Add(player.Location with { Row = player.Location.Row + 1 });
        if (player.Location.Column - 1 >= 0) _adjacentLocations.Add(player.Location with { Column = player.Location.Column - 1});
        if (player.Location.Column + 1 < cave.Columns) _adjacentLocations.Add(player.Location with { Column = player.Location.Column + 1});
    }
}