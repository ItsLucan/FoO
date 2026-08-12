namespace The_Fountain_of_Objects.Scripts;

public class Player(Location spawnLocation, int rows, int columns)
{
    public Location Location { get; private set; } = spawnLocation;
    private InputActions _inputAction;

    public void Teleport(Location location)
    {
        Location = location;
    }
    
    public void GetInput()
    {
        _inputAction = Console.ReadKey(true).Key switch
        {
            ConsoleKey.R                          => InputActions.Repair,
            ConsoleKey.W or ConsoleKey.UpArrow    => InputActions.MoveUp,
            ConsoleKey.A or ConsoleKey.LeftArrow  => InputActions.MoveLeft,
            ConsoleKey.S or ConsoleKey.DownArrow  => InputActions.MoveDown,
            ConsoleKey.D or ConsoleKey.RightArrow => InputActions.MoveRight,
            _                                     => InputActions.UnAccounted
        };
        
        CheckForMove();
    }

    public bool IsInputtingRepair()
    {
        if (_inputAction == InputActions.Repair) return true;

        return false;
    }
    
    private void CheckForMove()
    {
        Location desiredLocation = _inputAction switch
        {
            InputActions.MoveUp when Location.Row - 1 >= 0                          => Location with { Row = Location.Row - 1 },
            InputActions.MoveLeft when Location.Column - 1 >= 0                     => Location with { Column = Location.Column - 1 },
            InputActions.MoveDown when Location.Row + 1 < rows                      => Location with { Row = Location.Row + 1 },
            InputActions.MoveRight when Location.Column + 1 < columns               => Location with { Column = Location.Column + 1 },
            _                                                                       => Location
        };

        if (Location != desiredLocation) Location = desiredLocation;
    }
    
    private enum InputActions { MoveUp, MoveDown, MoveLeft, MoveRight, Repair, UnAccounted }
}