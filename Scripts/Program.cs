using The_Fountain_of_Objects.Scripts;

Console.CursorVisible = false;
Cave cave = new Cave();
Player player = new Player();
Game game = new Game(player, cave);

game.Run();