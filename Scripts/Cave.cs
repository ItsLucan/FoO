namespace The_Fountain_of_Objects;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    public FountainRoom FountainRoom { get; }
    public PitRoom PitRoom { get; }
    public Location MaelstromLocation { get; private set; }
    public Location AmarokLocation { get; private set; }
    private Randomizer _randomizer; 
    public Cave(Randomizer randomizer)
    {
        _randomizer = randomizer;
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

        Location fountainLocation = _randomizer.GetRandomLocationNoOverlap();
        Location pitLocation = _randomizer.GetRandomLocationNoOverlap();
        Location entranceLocation = new Location { Row = 0, Column = 0 };
        MaelstromLocation = _randomizer.GetRandomLocationNoOverlap();
        AmarokLocation = _randomizer.GetRandomLocationNoOverlap();
        
        FountainRoom = new FountainRoom(fountainLocation);
        PitRoom = new PitRoom(pitLocation);
        Rooms[0, 0] = new EntranceRoom(entranceLocation);
        
            AmarokLocation = _randomizer.GetRandomLocation();
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.Maelstrom);
        Rooms[AmarokLocation.Row, AmarokLocation.Column].SetEnemyHere(EnemyType.Amarok);
        Rooms[fountainLocation.Row, fountainLocation.Column] = FountainRoom;
        Rooms[pitLocation.Row, pitLocation.Column] = PitRoom;
    }

    public void MoveMaelstrom()
    {
        Rooms[MaelstromLocation.Row, MaelstromLocation.Column].SetEnemyHere(EnemyType.None);
        MaelstromLocation = _randomizer.GetRandomLocation();
        GetRoomAt(MaelstromLocation).SetEnemyHere(EnemyType.Maelstrom);
    } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}