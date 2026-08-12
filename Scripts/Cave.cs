namespace The_Fountain_of_Objects.Scripts;

public class Cave
{
    public Room[,] Rooms { get; }
    public int Rows { get; }
    public int Columns { get; }
    public Room EntranceRoom { get; }
    private readonly GameData _gameData;
    
    public Cave(GameData gameData)
    {
        _gameData = gameData;
        Rows = _gameData.Rows;
        Columns = _gameData.Columns;
        Rooms = new Room[Rows, Columns];
            
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                Rooms[row, column] = new Room(RoomType.Empty, new Location { Row = row, Column = column});
            }
        }
        
        Rooms[_gameData.EntranceLocation.Row, _gameData.EntranceLocation.Column] = new Room(RoomType.Entrance, _gameData.EntranceLocation);
        EntranceRoom = GetRoomAt(_gameData.EntranceLocation);
        Rooms[_gameData.FountainLocation.Row, _gameData.FountainLocation.Column] = new Room(RoomType.Fountain, _gameData.FountainLocation);
        foreach (Location location in _gameData.PitLocations) Rooms[location.Row, location.Column] = new Room(RoomType.Pit, location);
        foreach (Location location in _gameData.AmarokLocations) GetRoomAt(location).SetEnemyHere(EnemyType.Amarok);
        foreach (Location location in _gameData.MaelstromLocations) GetRoomAt(location).SetEnemyHere(EnemyType.Maelstrom);
    }

    public void MoveMaelstrom(Location maelstromLocation)
    {
        GetRoomAt(maelstromLocation).SetEnemyHere(EnemyType.None);
        GetRoomAt(_gameData.UpdateEnemyLocations(maelstromLocation)).SetEnemyHere(EnemyType.Maelstrom);
    }
    public Room GetRoomAt(Location location) => Rooms[location.Row, location.Column];
}