namespace The_Fountain_of_Objects;

public sealed class EmptyRoom(Location location) : Room(location)
{
    protected override TypeOfRoom RoomType { get; } = TypeOfRoom.Empty;
    public override EnemyType Enemy { get; private protected set; } = EnemyType.None;
    public override bool IsEnemySpawnable { get; private protected set; } = true;
}