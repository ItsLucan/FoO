namespace The_Fountain_of_Objects.Scripts;

public class Cave
{
    public Room[,] Rooms { get; }
    
    public Cave(int rows, int columns, Location entranceLocation, Location fountainLocation, Location[] pitLocations, Location[] amarokLocations)
    {
        Rooms = new Room[rows, columns];
            
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }


        
        Rooms[entranceLocation.Row, entranceLocation.Column] = new Room(RoomType.Entrance, entranceLocation);
        Rooms[fountainLocation.Row, fountainLocation.Column] = new Room(RoomType.Fountain, fountainLocation);
        foreach (Location location in pitLocations)
        {
            Rooms[location.Row, location.Column] = new Room(RoomType.Pit, location);    
        }

        foreach (Location location in amarokLocations)
        {
            GetRoomAt(location).SetEnemyHere(EnemyType.Amarok);
        }
    }

    public void MoveMaelstrom() { } 
    
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}