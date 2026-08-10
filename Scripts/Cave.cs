namespace The_Fountain_of_Objects.Scripts;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    private Location _maelstromLocation;
    
    public Cave()
    {
        Rows = Randomizer.MaxRows;
        Columns = Randomizer.MaxColumns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }
        
        Location entranceLocation = new Location { Row = 0, Column = 0 };
        Location fountainLocation = Randomizer.GetSafeRandomLocation();
        Location pitLocation = Randomizer.GetSafeRandomLocation();
        _maelstromLocation = Randomizer.GetSafeRandomLocation();
        Location amarokLocation = Randomizer.GetSafeRandomLocation();
        
        
        Rooms[_maelstromLocation.Row, _maelstromLocation.Column].SetEnemyHere(EnemyType.Maelstrom);
        Rooms[amarokLocation.Row, amarokLocation.Column].SetEnemyHere(EnemyType.Amarok);
        Rooms[entranceLocation.Row, entranceLocation.Column] = new Room(RoomType.Entrance, entranceLocation);
        Rooms[fountainLocation.Row, fountainLocation.Column] = new Room(RoomType.Fountain, fountainLocation);
        Rooms[pitLocation.Row, pitLocation.Column] = new Room(RoomType.Pit, pitLocation);
    }

    public void MoveMaelstrom()
    {
        Rooms[_maelstromLocation.Row, _maelstromLocation.Column].SetEnemyHere(EnemyType.None);
        _maelstromLocation = Randomizer.GetRandomLocation();
        GetRoomAt(_maelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
    } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}