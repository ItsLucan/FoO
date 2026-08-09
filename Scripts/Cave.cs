namespace The_Fountain_of_Objects;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    private Location _fountainLocation;
    private Location _pitLocation;

    public FountainRoom FountainRoom { get; }
    public PitRoom PitRoom { get; }
    
    private Randomizer _randomizer = new Randomizer();
    public Cave()
    {
        Rows = _randomizer.MaxRows;
        Columns = _randomizer.MaxColumns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < Rooms.GetLength(1); column++)
            {
                Rooms[row, column] = new Room(new Location { Row = row, Column = column});
            }
        }

        _fountainLocation = _randomizer.GetRandomLocation();
        _pitLocation = _randomizer.GetRandomLocation();
        FountainRoom = new FountainRoom(_fountainLocation);
        PitRoom = new PitRoom(_pitLocation);
        Rooms[0, 0] = new EntranceRoom(new Location { Row = 0, Column = 0 });
        Rooms[_fountainLocation.Row, _fountainLocation.Column] = FountainRoom;
        Rooms[_pitLocation.Row, _pitLocation.Column] = PitRoom;
    }
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}