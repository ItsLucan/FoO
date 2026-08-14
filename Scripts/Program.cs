using The_Fountain_of_Objects.Scripts;

Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;
GameData gameData = new GameData();
Player player = new Player(gameData);
Cave cave = new Cave(gameData);
Game game = new Game(player, cave);

game.Run();