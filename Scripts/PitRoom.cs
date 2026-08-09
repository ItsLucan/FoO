namespace The_Fountain_of_Objects.Scripts;

public sealed class PitRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Pit;
    
}