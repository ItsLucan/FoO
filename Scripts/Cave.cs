namespace The_Fountain_of_Objects.Scripts;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    public FountainRoom FountainRoom { get; }
    public PitRoom PitRoom { get; }
    public Location MaelstromLocation { get; private set; }
    public Location AmarokLocation { get; }

    public Cave()
    {
        Rows = Randomizer.MaxRows;
        Columns = Randomizer.MaxColumns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new EmptyRoom(new Location { Row = row, Column = column});
            }
        }

        Location fountainLocation = Randomizer.GetSafeRandomLocation();
        Location pitLocation = Randomizer.GetSafeRandomLocation();
        Location entranceLocation = new Location { Row = 0, Column = 0 };
        MaelstromLocation = Randomizer.GetSafeRandomLocation();
        AmarokLocation = Randomizer.GetSafeRandomLocation();
        
        FountainRoom = new FountainRoom(fountainLocation);
        PitRoom = new PitRoom(pitLocation);
        Rooms[0, 0] = new EntranceRoom(entranceLocation);
        
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.Maelstrom);
        Rooms[AmarokLocation.Row, AmarokLocation.Column].SetEnemyHere(EnemyType.Amarok);
        Rooms[fountainLocation.Row, fountainLocation.Column] = FountainRoom;
        Rooms[pitLocation.Row, pitLocation.Column] = PitRoom;
    }

    public void MoveMaelstrom()
    {
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.None);
        MaelstromLocation = Randomizer.GetRandomLocation();
        GetRoomAt(MaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
    } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}