namespace The_Fountain_of_Objects;

public class PitRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Pit;
}