namespace The_Fountain_of_Objects;

public class FountainRoom(Location location) : Room(location)
{
    protected override RoomType Type { get; } = RoomType.Fountain;
    public bool IsRepaired { get; private set; }
    public void Repair()
    {
        IsRepaired = true;
    }
}