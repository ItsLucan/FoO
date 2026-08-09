namespace The_Fountain_of_Objects.Scripts;

public sealed class EntranceRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Entrance;
}