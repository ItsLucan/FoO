namespace The_Fountain_of_Objects;

public class Game
{
    private Player _player;
    private Cave _cave;
    private Sense _sense;
    private Room _currentRoom;
    public Game(Player player, Cave cave)
    {
        _player = player;
        _cave = cave;
        _sense = new Sense(player, cave);
        _currentRoom = _cave.GetRoomAt(_player.Location);
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            UpdatePlayerRoom();
            Display();
            Console.WriteLine(_player.Location);
            Console.WriteLine(_currentRoom);
            _sense.Display();
            if (_currentRoom is PitRoom)
            {
                Console.ReadKey(true);
                return;
            }
           
            if (_currentRoom is EntranceRoom && _cave.FountainRoom.IsRepaired)
            {
                Console.ReadKey(true);
                return;
            }
            _player.GetInput();
            
            if (_currentRoom is FountainRoom && _player.IsInputtingRepair())
            {
                _cave.FountainRoom.Repair();
            }
        }
    }
    
    private void UpdatePlayerRoom()
    {
        if (!_player.Location.Equals(_currentRoom.Location))
        {
            _currentRoom.SetPlayerHere(false);
            _currentRoom = _cave.GetRoomAt(_player.Location);
        }

        _currentRoom.SetPlayerHere(true);
    }
    
    private void Display()
    {
        for (int row = 0; row < _cave.Rooms.GetLength(0); row++)
        {
            for (int column = 0; column < _cave.Rooms.GetLength(1); column++)
            {
                Console.Write(_cave.Rooms[row, column].IsPlayerHere ? "o " : "# ");
            }

            Console.WriteLine();
        }
    }
}