namespace The_Fountain_of_Objects;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    public FountainRoom FountainRoom { get; }
    public PitRoom PitRoom { get; }
    public Location MaelstromLocation { get; private set; }
    
    public Cave(Randomizer randomizer)
    {
        Randomizer _randomizer = randomizer;
        Rows = _randomizer.MaxRows;
        Columns = _randomizer.MaxColumns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new EmptyRoom(new Location { Row = row, Column = column});
            }
        }

        Location fountainLocation = _randomizer.GetRandomRoomSpawnLocation();
        Location pitLocation = _randomizer.GetRandomRoomSpawnLocation();
        Location entranceLocation = new Location { Row = 0, Column = 0 };
        MaelstromLocation = _randomizer.GetRandomRoomSpawnLocation();
        
        FountainRoom = new FountainRoom(fountainLocation);
        PitRoom = new PitRoom(pitLocation);
        Rooms[0, 0] = new EntranceRoom(entranceLocation);
        
        while (!GetRoomAt(MaelstromLocation).IsEnemySpawnable)
        {
            MaelstromLocation = _randomizer.GetRandomRoomSpawnLocation();
        }
        
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.Maelstrom);
        Rooms[fountainLocation.Row, fountainLocation.Column] = FountainRoom;
        Rooms[pitLocation.Row, pitLocation.Column] = PitRoom;
    }

    public void MoveMaelstrom(Location location)
    {
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.None);
        MaelstromLocation = location;
        GetRoomAt(location).SetEnemyHere(EnemyType.Maelstrom);
    } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}