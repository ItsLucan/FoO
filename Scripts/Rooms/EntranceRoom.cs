namespace The_Fountain_of_Objects;

public class EntranceRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Entrance;
}