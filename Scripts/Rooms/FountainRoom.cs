namespace The_Fountain_of_Objects;

public sealed class FountainRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Fountain;
    public bool IsRepaired { get; private set; }
    public void Repair()
    {
        IsRepaired = true;
    }
}