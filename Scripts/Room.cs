namespace The_Fountain_of_Objects.Scripts;


public class Room(RoomType roomType)
{
    public RoomType RoomType { get; } = roomType;
    public EnemyType EnemyType { get; private set; } = EnemyType.None;
    public bool IsPlayerHere { get; private set; }
    public bool HasPlayerVisited { get; private set; }

    
    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere; 
        if (!HasPlayerVisited) HasPlayerVisited = true;    
    }
   
    
    public void SetEnemyHere(EnemyType enemyType) => EnemyType = enemyType;
}