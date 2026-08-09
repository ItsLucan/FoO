using The_Fountain_of_Objects.Scripts;

Cave cave = new Cave();
Player player = new Player(cave.Rows, cave.Columns);
Game game = new Game(player, cave);

game.Run();