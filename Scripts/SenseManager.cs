namespace The_Fountain_of_Objects.Scripts;


public static class SenseManager
{
    private const string See           = $"\e[38;2;251;245;43mSEE:{ResetColor}";
    private const string Hear          = $"\e[38;2;137;251;43mHEAR:{ResetColor}";
    private const string Feel          = $"\e[38;2;194;49;160mFEEL:{ResetColor}";
    private const string Smell         = $"\e[38;2;251;159;43mSMELL:{ResetColor}";
    private const string NegativeColor = "\e[38;2;172;48;0m";
    private const string FountainColor = "\e[38;2;60;120;255m";
    private const string ResetColor    = "\e[39m";
    
    
    public static void DisplayCurrentRoomSense(Room currentRoom, bool isFountainRepaired)
    {
        string? currentRoomText = currentRoom.RoomType switch
        {
            RoomType.Entrance when isFountainRepaired  => $"{Feel} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.\n\nPress any key to quit.", 
            RoomType.Fountain when isFountainRepaired  => $"{Hear} Water rushing from the {FountainColor}Fountain of objects{ResetColor}. It is repaired.",
            RoomType.Fountain                          => $"{See} The silhouette of a large {FountainColor}fountain{ResetColor}. You are in the fountain room.",
            RoomType.Entrance                          => $"{See} light from outside the cave. You are at the entrance.",
            RoomType.Pit                               => $"{NegativeColor}GAME OVER:{ResetColor} You step onto ground with no substance, and tumble into a vast chasm.\n\nPress any key to quit.",
            RoomType.Empty                             => null,
            _                                          => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };
        
        if (currentRoomText is not null) Console.WriteLine(currentRoomText); 
    }
   
    
    public static void DisplayCurrentEnemySense(Room currentRoom)
    {
        string? currentEnemyText = currentRoom.EnemyType switch
        {
            EnemyType.Maelstrom => $"{Feel} The torrential strength of a {NegativeColor}Maelstrom{ResetColor}. You both are sent flying through the cave.\n\nPress any key to continue.",
            EnemyType.Amarok    => $"{NegativeColor}GAME OVER:{ResetColor} You waltz into the maw of a foul {NegativeColor}Amarok{ResetColor}, who rends your flesh. You have died.\n\nPress any key to quit.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        if (currentEnemyText is not null) Console.WriteLine(currentEnemyText);
    }

    
    public static void DisplayAdjacentRoomSenseAt(Room adjacentRoom)
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

    
    public static void DisplayAdjacentEnemySenseAt(Room adjacentRoom)
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
}