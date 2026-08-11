namespace The_Fountain_of_Objects.Scripts;

public class Cave
{
    public Room[,] Rooms { get; }
    private Location _firstMaelstromLocation;
    private Location _secondMaelstromLocation;
    public Cave()
    {
        Rooms = new Room[Randomizer.Rows, Randomizer.Columns];
            
        for (int row = 0; row < Randomizer.Rows; row++)
        {
            for (int column = 0; column < Randomizer.Columns; column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }

        Location fountainLocation = Randomizer.GetSafeRandomLocation();
        Location pitLocation = Randomizer.GetSafeRandomLocation();
        _firstMaelstromLocation = Randomizer.GetSafeRandomLocation();
        _secondMaelstromLocation = Randomizer.GetSafeRandomLocation();
        Location firstAmarokLocation = Randomizer.GetSafeRandomLocation();
        Location secondAmarokLocation = Randomizer.GetSafeRandomLocation();

        
        Rooms[Randomizer.EntranceLocation.Row, Randomizer.EntranceLocation.Column] = new Room(RoomType.Entrance, Randomizer.EntranceLocation);
        Rooms[fountainLocation.Row, fountainLocation.Column] = new Room(RoomType.Fountain, fountainLocation);
        Rooms[pitLocation.Row, pitLocation.Column] = new Room(RoomType.Pit, pitLocation);
        GetRoomAt(_firstMaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
        GetRoomAt(_secondMaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
        GetRoomAt(firstAmarokLocation).SetEnemyHere(EnemyType.Amarok);
        GetRoomAt(secondAmarokLocation).SetEnemyHere(EnemyType.Amarok);
    }

    public void MoveMaelstrom(Location playerLocation)
    {
        Rooms[playerLocation.Row, playerLocation.Column].SetEnemyHere(EnemyType.None);
        if (playerLocation == _firstMaelstromLocation)
        {
            _firstMaelstromLocation = Randomizer.GetRandomLocation();    
            GetRoomAt(_firstMaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
        }
        else
        {
            _secondMaelstromLocation = Randomizer.GetRandomLocation();
            GetRoomAt(_secondMaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
        }
    } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}