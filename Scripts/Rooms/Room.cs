namespace The_Fountain_of_Objects;

public class Room
{
    protected virtual RoomType Type { get; } = RoomType.Empty;
    public bool IsPlayerHere { get; private set; }
    public Location Location { get; }

    public Room(Location location)
    {
        Location = location;
    }
    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere;
    }
    
    protected enum RoomType { Empty, Fountain, Pit, Entrance }
}