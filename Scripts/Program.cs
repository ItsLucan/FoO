using The_Fountain_of_Objects;


Randomizer randomizer = new Randomizer();
Cave cave = new Cave(randomizer);
Player player = new Player(cave.Rows, cave.Columns);
Game game = new Game(player, cave, randomizer);

game.Run();