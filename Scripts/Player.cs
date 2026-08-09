namespace The_Fountain_of_Objects.Scripts;

public class Player(int caveRows, int caveColumns)
{
    public Location Location { get; private set; } = new Location { Row = 0, Column = 0};
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
        Location? desiredLocation = new Location(Location.Row, Location.Column);
        desiredLocation = _inputAction switch
        {
            InputActions.Repair or InputActions.UnAccounted => null,
            InputActions.MoveUp when Location.Row - 1 >= 0                => Location with { Row = Location.Row - 1 },
            InputActions.MoveLeft when Location.Column - 1 >= 0           => Location with { Column = Location.Column - 1 },
            InputActions.MoveDown when Location.Row + 1 < caveRows        => Location with { Row = Location.Row + 1 },
            InputActions.MoveRight when Location.Column + 1 < caveColumns => Location with { Column = Location.Column + 1 },
            _                                                             => Location
        };

        if (desiredLocation is not null)
        {
            Location = (Location)desiredLocation;
        }
    }
    
    private enum InputActions { MoveUp, MoveDown, MoveLeft, MoveRight, Repair, UnAccounted }
}