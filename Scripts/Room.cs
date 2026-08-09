namespace The_Fountain_of_Objects.Scripts;

public class Room(RoomType roomType, Location location)
{
    public RoomType RoomType { get; } = roomType;
    public Location Location { get; } = location;
    public EnemyType EnemyType { get; private set; } = EnemyType.None;
    public bool IsPlayerHere { get; private set; } = false;
    
    
    public void SetPlayerHere(bool isHere) => IsPlayerHere = isHere;

    public void SetEnemyHere(EnemyType enemyType) => EnemyType = enemyType;
}