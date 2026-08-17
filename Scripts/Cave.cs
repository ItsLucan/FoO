namespace The_Fountain_of_Objects.Scripts;


public class Cave
{
    public int Rows { get; }
    public int Columns { get; }
    private readonly GameData _gameData;
    private readonly Room[,] _rooms;
    
    
    public Cave(GameData gameData)
    {
        _gameData = gameData;
        Rows = _gameData.Rows;
        Columns = _gameData.Columns;
        _rooms = new Room[Rows, Columns];
        
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                _rooms[row, column] = new Room(RoomType.Empty);
            }
        }
        
        _rooms[_gameData.EntranceLocation.Row, _gameData.EntranceLocation.Column] = new Room(RoomType.Entrance);
        _rooms[_gameData.FountainLocation.Row, _gameData.FountainLocation.Column] = new Room(RoomType.Fountain);
        foreach (Location location in _gameData.PitLocations) _rooms[location.Row, location.Column] = new Room(RoomType.Pit);
        foreach (Location location in _gameData.AmarokLocations) GetRoomAt(location).SetEnemyHere(EnemyType.Amarok);
        foreach (Location location in _gameData.MaelstromLocations) GetRoomAt(location).SetEnemyHere(EnemyType.Maelstrom);
    }

    
    public void TeleportMaelstromAt(Location maelstromLocation)
    {
        GetRoomAt(maelstromLocation).SetEnemyHere(EnemyType.None);
        Room newMaelstromRoom = GetRoomAt(_gameData.ChangeMaelstromLocation(maelstromLocation));
        newMaelstromRoom.SetEnemyHere(EnemyType.Maelstrom);
    }
   
    
    public Room GetRoomAt(Location location) => _rooms[location.Row, location.Column];
}