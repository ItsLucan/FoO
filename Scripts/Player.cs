namespace The_Fountain_of_Objects;

public class Player(int caveRows, int caveColumns)
{
    public Location Location { get; private set; } = new Location { Row = 0, Column = 0};
    private ConsoleKey GetKeyPress() => Console.ReadKey(true).Key;
    private InputActions _inputAction;
    public void GetInput()
    {
        _inputAction = GetKeyPress() switch
        {
            ConsoleKey.R => InputActions.Repair,
            ConsoleKey.W => InputActions.MoveUp,
            ConsoleKey.A => InputActions.MoveLeft,
            ConsoleKey.S => InputActions.MoveDown,
            ConsoleKey.D => InputActions.MoveRight,
            _ => InputActions.UnAccounted
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
            InputActions.Repair => null,
            InputActions.MoveUp when Location.Row - 1 >= 0 => Location with { Row = Location.Row - 1 },
            InputActions.MoveLeft when Location.Column - 1 >= 0 => Location with { Column = Location.Column - 1 },
            InputActions.MoveDown when Location.Row + 1 < caveRows => Location with { Row = Location.Row + 1 },
            InputActions.MoveRight when Location.Column + 1 < caveColumns => Location with { Column = Location.Column + 1 },
            _ => Location
        };

        if (desiredLocation is not null)
        {
            Location = (Location)desiredLocation;
        }
    }
    
    private enum InputActions { MoveUp, MoveDown, MoveLeft, MoveRight, Repair, UnAccounted }
}