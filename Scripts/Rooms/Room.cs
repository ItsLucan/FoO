namespace The_Fountain_of_Objects;

public abstract class Room(Location location)
{
    protected abstract TypeOfRoom RoomType { get; }
    public virtual EnemyType Enemy { get; private protected set; } = EnemyType.None;
    public virtual bool IsEnemySpawnable { get; private protected set; } = false; 
    public bool IsPlayerHere { get; private set; }
    public Location Location { get; } = location;
    
    public void SetPlayerHere(bool isHere)
    {
        IsPlayerHere = isHere;
    }

    public void SetEnemyHere(EnemyType enemy)
    {
        Enemy = enemy;
    }
        
    protected enum TypeOfRoom { Empty, Fountain, Pit, Entrance }
}