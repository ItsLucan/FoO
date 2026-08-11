namespace The_Fountain_of_Objects.Scripts;

public class Sensor()
{
    private const string SeeColor      = "\e[38;2;251;245;43m";
    private const string HearColor     = "\e[38;2;137;251;43m";
    private const string FeelColor     = "\e[38;2;194;49;160m";
    private const string SmellColor    = "\e[38;2;251;159;43m";
    private const string NegativeColor = "\e[38;2;172;48;0m";
    private const string FountainColor = "\e[38;2;60;120;255m";
    private const string ResetColor    = "\e[39m";
    
    public void DisplayCurrentSense(Room currentRoom, bool isFountainRepaired)
    {
        (string? currentRoomText, string? currentEnemyText) = GetCurrentSenses(currentRoom, isFountainRepaired);
        if (currentRoomText != null) Console.WriteLine(currentRoomText);
        if (currentEnemyText != null) Console.WriteLine(currentEnemyText);
    }
    
    public void DisplayAdjacentSenses(List<Room> adjacentRooms)
    {
        foreach (Room room in adjacentRooms)
        {
            (string? adjacentRoomText, string? adjacentEnemyText) = GetAdjacentSenses(room);
            if (adjacentRoomText != null) Console.WriteLine(adjacentRoomText);
            if (adjacentEnemyText != null) Console.WriteLine(adjacentEnemyText);
        }
    }
    
    private (string? currentRoomText, string? currentEnemyText) GetCurrentSenses(Room currentRoom, bool isFountainRepaired)
    {
        
        string? currentRoomText = currentRoom.RoomType switch
        {
            RoomType.Entrance when isFountainRepaired  => $"{FeelColor}FEEL:{ResetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.", 
            RoomType.Fountain when isFountainRepaired  => $"{HearColor}HEAR:{ResetColor} Water rushing from the {FountainColor}Fountain of objects{ResetColor}. It is repaired.",
            RoomType.Fountain                          => $"{SeeColor}SEE:{ResetColor} The silhouette of a large {FountainColor}fountain{ResetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{SeeColor}SEE:{ResetColor} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{NegativeColor}GAME OVER:{ResetColor} You step onto ground with no substance, and tumble into a vast chasm. You died.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        string? currentEnemyText = currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{FeelColor}FEEL:{ResetColor} The torrential strength of a Maelstrom. You both are sent flying through the cave.",
            EnemyType.Amarok    => $"{NegativeColor}GAME OVER:{ResetColor} You waltz into the maw of a foul Amarok, who rends your flesh. You have died.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        return (currentRoomText, currentEnemyText);
    }

    private (string? adjacentRoomText, string? adjacentEnemyText) GetAdjacentSenses(Room adjacentRoom)
    {
        string? adjacentRoomText = adjacentRoom.RoomType switch
        {
            RoomType.Fountain                   => $"{HearColor}HEAR:{ResetColor} A faint dripping in the distance. The {FountainColor}fountain{ResetColor} is close.",
            RoomType.Pit                        => $"{FeelColor}FEEL:{ResetColor} The howling breath of a hungry chasm. A {NegativeColor}pit{ResetColor} is nearby.",
            RoomType.Entrance or RoomType.Empty => null,    
            _                                   => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        string? adjacentEnemyText = adjacentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{FeelColor}FEEL:{ResetColor} The ghastly winds of a {NegativeColor}Maelstrom{ResetColor} nearby.",
            EnemyType.Amarok    => $"{SmellColor}SMELL:{ResetColor} The pungent odor of rotten flesh. An {NegativeColor}Amarok{ResetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        };
        
        return (adjacentRoomText, adjacentEnemyText);
    }
}