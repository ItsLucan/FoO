namespace The_Fountain_of_Objects;

public class Sense(Player player, Cave cave)
{
    private string seeColor = \e[38;2;251;245;43m;
    private string hearColor = \e[38;2;137;251;43m;
    private string feelColor = \e[38;2;194;49;160m;
    private string smellColor = \e[38;2;251;159;43m;
    private string negativeColor = \e[38;2;172;48;0m;
    private string fountainColor = \e[38;2;60;120;255m;
    private string resetColor = \e[39m;
    private List<Room> _adjacentRooms;
    private List<Location> _adjacentLocations;
    
    public void Display()
    {
        SetAdjacentLocations();
        SetAdjacentRooms();
        (string? currentRoomTypeSense, string? currentEnemyTypeSense) = GetCurrentSenses();
        if (currentRoomTypeSense != null)
        {
            Console.WriteLine(currentRoomTypeSense);
        }
        if (currentEnemyTypeSense != null)
        {
            Console.WriteLine(currentEnemyTypeSense);
        }
        foreach (Room room in _adjacentRooms)
        {
            (string? adjacentRoomSense, string? adjacentEnemySense) = GetAdjacentRoomTypeSense(room);
            
            if (adjacentRoomSense != null)
            {
                Console.WriteLine(adjacentRoomSense);
            }

            if (adjacentEnemySense != null)
            {
                Console.WriteLine(adjacentEnemySense);
            }
        }
    }

    private (string? currentRoom, string? currentEnemy) GetCurrentSenses()
    {
        Room currentRoom = cave.GetRoomAt(player.Location);
        
        string? currentRoomSense = currentRoom switch
        {
            EntranceRoom when cave.FountainRoom.IsRepaired => $"{feelColor}FEEL:{resetColor} The warm embrace of the free sun through the caves entrance. You have conquered The Uncoded Ones challenge.", 
            FountainRoom when cave.FountainRoom.IsRepaired => $"{hearColor}HEAR:{resetColor} Water rushing from the {fountainColor}Fountain of objects{resetColor}. It is repaired.",
            FountainRoom                                   => $"{seeColor}SEE:{resetColor} You see the silhouette of a large {fountainColor}fountain{resetColor}. You are in the fountain room.",
            EntranceRoom                                   => $"{seeColor}SEE:{resetColor} light from outside the cave. You are at the entrance.",
            PitRoom                                        => $"{negativeColor}GAME OVER:{resetColor} You lose your footing and tumble into a vast chasm. You have died.",
            EmptyRoom                                      => null,
            _                                              => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        string? currentEnemySense = currentRoom.Enemy switch
        {
            
            EnemyType.Maelstrom => $"{hearColor}HEAR:{resetColor} torrential storm of a Maelstrom. You both are sent flying through the cave.",
            EnemyType.Amarok    => $"{negativeColor}GAME OVER:{resetColor} You waltz into the maw of a foul Amarok, who rends your flesh. You have died.",
            EnemyType.None      => null,
            _                   => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        return (currentRoomSense, currentEnemySense);
    }

    private (string? adjacentRoom, string? adjacentEnemy) GetAdjacentRoomTypeSense(Room currentRoom)
    {
        string? adjacentRoom = currentRoom switch
        {
            FountainRoom              => $"{hearColor}HEAR:{resetColor} a faint dripping nearby. The {fountainColor}fountain{resetColor} is close.",
            PitRoom                   => $"{feelColor}FEEL:{resetColor} The howling winds of a hungry chasm. A {negativeColor}pit{resetColor} is nearby.",
            EntranceRoom or EmptyRoom => null,    
            _                         => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        string? adjacentEnemy = currentRoom.Enemy switch
        {
            EnemyType.Maelstrom => $"{feelColor}FEEL:{resetColor} The ghastly winds of a {negativeColor}Maelstrom{negativeColor} nearby.",
            EnemyType.Amarok    => $"{smellColor}SMELL:{resetColor} You smell the pungent odor of rotten flesh. An {negativeColor}Amarok{resetColor} is nearby.",
            EnemyType.None      => null,
            _                   => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
        };
        return (adjacentRoom, adjacentEnemy);
    }
    
    private void SetAdjacentRooms()
    {
        _adjacentRooms = new List<Room>();
        
        foreach (Location location in _adjacentLocations)
        {
            _adjacentRooms.Add(cave.GetRoomAt(location));
        }
    }
    
    private void SetAdjacentLocations()
    {
        _adjacentLocations = new List<Location>();
        if (player.Location.Row - 1 >= 0)
        {
            _adjacentLocations.Add(player.Location with { Row = player.Location.Row - 1 });
        }

        if (player.Location.Row + 1 < cave.Rows)
        {
            _adjacentLocations.Add(player.Location with { Row = player.Location.Row + 1 });
        }

        if (player.Location.Column - 1 >= 0)
        {
            _adjacentLocations.Add(player.Location with { Column = player.Location.Column - 1});
        }

        if (player.Location.Column + 1 < cave.Columns)
        {
            _adjacentLocations.Add(player.Location with{ Column = player.Location.Column + 1});
        }
    }
}