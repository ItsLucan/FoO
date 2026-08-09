namespace The_Fountain_of_Objects;

public sealed class PitRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Pit;
    
}