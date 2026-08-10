namespace The_Fountain_of_Objects.Scripts;

public class Sensor()
{
    private string _seeColor      = "\e[38;2;251;245;43m";
    private string _hearColor     = "\e[38;2;137;251;43m";
    private string _feelColor     = "\e[38;2;194;49;160m";
    private string _smellColor    = "\e[38;2;251;159;43m";
    private string _negativeColor = "\e[38;2;172;48;0m";
    private string _fountainColor = "\e[38;2;60;120;255m";
    private string _resetColor    = "\e[39m";
    
    public void DisplayCurrentSense(Room currentRoom, bool isFountainRepaired)
    {
        (string? currentRoomSense, string? currentEnemySense) = GetCurrentSenses(currentRoom, isFountainRepaired);
        if (currentRoomSense != null) Console.WriteLine(currentRoomSense);
        if (currentEnemySense != null) Console.WriteLine(currentEnemySense);
    }
    
    public void DisplayAdjacentSenses(List<Room> adjacentRooms)
    {
        foreach (Room room in adjacentRooms)
        {
            (string? adjacentRoomSense, string? adjacentEnemySense) = GetAdjacentSenses(room);
            if (adjacentRoomSense != null) Console.WriteLine(adjacentRoomSense);
            if (adjacentEnemySense != null) Console.WriteLine(adjacentEnemySense);
        }
    }
    
    private (string? currentRoomMessage, string? currentEnemyMessage) GetCurrentSenses(Room currentRoom, bool isFountainRepaired)
    {
        
        string? currentRoomSense = currentRoom.RoomType switch
        {
            RoomType.Entrance when isFountainRepaired  => $"{_feelColor}FEEL:{_resetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.", 
            RoomType.Fountain when isFountainRepaired  => $"{_hearColor}HEAR:{_resetColor} Water rushing from the {_fountainColor}Fountain of objects{_resetColor}. It is repaired.",
            RoomType.Fountain                          => $"{_seeColor}SEE:{_resetColor} The silhouette of a large {_fountainColor}fountain{_resetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{_seeColor}SEE:{_resetColor} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{_negativeColor}GAME OVER:{_resetColor} You step onto ground with no substance, and tumble into a vast chasm. You died.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
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

    private (string? adjacentRoom, string? adjacentEnemy) GetAdjacentSenses(Room adjacentRoom)
    {
        string? adjacentRoomSense = adjacentRoom.RoomType switch
        {
            RoomType.Fountain                   => $"{_hearColor}HEAR:{_resetColor} A faint dripping in the distance. The {_fountainColor}fountain{_resetColor} is close.",
            RoomType.Pit                        => $"{_feelColor}FEEL:{_resetColor} The howling breath of a hungry chasm. A {_negativeColor}pit{_resetColor} is nearby.",
            RoomType.Entrance or RoomType.Empty => null,    
            _                                   => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        string? adjacentEnemySense = adjacentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{_feelColor}FEEL:{_resetColor} The ghastly winds of a {_negativeColor}Maelstrom{_resetColor} nearby.",
            EnemyType.Amarok    => $"{_smellColor}SMELL:{_resetColor} The pungent odor of rotten flesh. An {_negativeColor}Amarok{_resetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        };
        
        return (adjacentRoomSense, adjacentEnemySense);
    }
}