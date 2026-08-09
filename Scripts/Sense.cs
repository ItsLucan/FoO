namespace The_Fountain_of_Objects;

public class Sense(Player player, Cave cave)
{
    private List<Room> _adjacentRooms;
    private List<Location> _adjacentLocations;
    
    public void Display()
    {
        SetAdjacentLocations();
        SetAdjacentRooms();
        string? currentSense = GetCurrentSense();
        string? adjacentSense;
        if (currentSense != null)
        {
            Console.WriteLine(currentSense);
        }
        
        foreach (Room room in _adjacentRooms)
        {
            adjacentSense = GetAdjacentSense(room);
            if (adjacentSense != null)
            {
                Console.WriteLine(adjacentSense);
            }
        }
    }

    private string? GetCurrentSense()
    {
        Room currentRoom = cave.GetRoomAt(player.Location);
        
        string? currentRoomSense = currentRoom switch
        {
            EntranceRoom when cave.FountainRoom.IsRepaired => "You see a light in front of you and escape the cave. You have conquered The Uncoded Ones challenge.", 
            EntranceRoom => "You see light from outside the cave. You are at the entrance.",
            FountainRoom when cave.FountainRoom.IsRepaired => "Water rushes from the Fountain of objects. It is repaired.",
            FountainRoom => "You see the silhouette of a large fountain. You are in the fountain room.",
            PitRoom => "You lose your footing and tumble into a vast chasm. You have died.",
            Room => null,
            _ => "ERROR: CURRENT ROOM UNACCOUNTED FOR."
        };

        return currentRoomSense;
    }

    private string? GetAdjacentSense(Room currentRoom)
    {
        string? adjacentSense = currentRoom switch
        {
            EntranceRoom => null,
            FountainRoom => "You hear a faint dripping nearby. The fountain is close.",
            PitRoom => "You hear the howling of a hungry chasm. A pit is nearby.",
            Room => null,
            _ => "ERROR: ADJACENT ROOM UNACCOUNTED FOR."
        };

        return adjacentSense;
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