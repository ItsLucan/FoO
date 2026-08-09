namespace The_Fountain_of_Objects;

public class Sense(Player player, Cave cave)
{
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
            EntranceRoom when cave.FountainRoom.IsRepaired => "You see a light in front of you and escape the cave. You have conquered The Uncoded Ones challenge.", 
            EntranceRoom => "You see light from outside the cave. You are at the entrance.",
            FountainRoom when cave.FountainRoom.IsRepaired => "Water rushes from the Fountain of objects. It is repaired.",
            FountainRoom => "You see the silhouette of a large fountain. You are in the fountain room.",
            PitRoom => "You lose your footing and tumble into a vast chasm. You have died.",
            EmptyRoom => null,
            _ => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        string? currentEnemySense = currentRoom.Enemy switch
        {
            EnemyType.None => null,
            EnemyType.Maelstrom => "You stumble into the torrential storm of a Maelstrom. You both are sent flying through the cave.",
            EnemyType.Amarok => "You waltz into the maw of a foul Amarok, who rends your flesh. You have died.",
            _ => "ERROR: CURRENT ENEMY UNACCOUNTED FOR."
        };

        return (currentRoomSense, currentEnemySense);
    }

    private (string? adjacentRoom, string? adjacentEnemy) GetAdjacentRoomTypeSense(Room currentRoom)
    {
        string? adjacentRoom = currentRoom switch
        {
            EntranceRoom => null,
            FountainRoom => "You hear a faint dripping nearby. The fountain is close.",
            PitRoom => "You hear the howling of a hungry chasm. A pit is nearby.",
            EmptyRoom => null,
            _ => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        string? adjacentEnemy = currentRoom.Enemy switch
        {
            EnemyType.None => null,
            EnemyType.Maelstrom => "You hear the ghastly wailing of a Maelstrom nearby.",
            EnemyType.Amarok => "You smell the pungent odor of rotten flesh. An Amarok is nearby.",
            _ => "ERROR: ADJACENT ENEMY UNACCOUNTED FOR."
            
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