namespace The_Fountain_of_Objects;

public sealed class EntranceRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Entrance;
}