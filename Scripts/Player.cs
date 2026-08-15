namespace The_Fountain_of_Objects.Scripts;

public class Player(GameData gameData)
{
    public Location Location { get; private set; } = gameData.EntranceLocation;
    public Location? ArrowLocation { get; private set; }
    private int _arrows = 5;
    private InputActions _inputAction;
    
    public void Teleport()
    {
        Location = gameData.GetRandomLocation();
    }
    
    public void GetInput()
    {
        ProcessKeyPress();
        if (IsInputtingShoot())
        {
            if (_arrows > 0)
            {
                _arrows--;
            }
            CheckForArrowMove();
            ProcessKeyPress();
        }
        Location = MutateOnMove(Location);
    }

    private bool IsInputtingShoot()
    { 
        return _inputAction == InputActions.Shoot;
    }
    
    public bool IsInputtingRepair()
    {
        return _inputAction == InputActions.Repair;
    }

    public bool IsOpeningMenu()
    {
        return _inputAction == InputActions.Menu;
    }

    private void ProcessKeyPress()
    {
        _inputAction = Console.ReadKey(true).Key switch
        {
            ConsoleKey.R                          => InputActions.Repair,
            ConsoleKey.Spacebar                   => InputActions.Shoot,
            ConsoleKey.Tab                        => InputActions.Menu,
            ConsoleKey.W or ConsoleKey.UpArrow    => InputActions.MoveUp,
            ConsoleKey.A or ConsoleKey.LeftArrow  => InputActions.MoveLeft,
            ConsoleKey.S or ConsoleKey.DownArrow  => InputActions.MoveDown,
            ConsoleKey.D or ConsoleKey.RightArrow => InputActions.MoveRight,
            _                                     => InputActions.UnAccounted
        };
    }
    
    private Location MutateOnMove(Location location)
    {
        Location desiredLocation = _inputAction switch
        {
            InputActions.MoveUp when location.Row - 1 >= 0                          => location with { Row = location.Row - 1 },
            InputActions.MoveLeft when location.Column - 1 >= 0                     => location with { Column = location.Column - 1 },
            InputActions.MoveDown when location.Row + 1 < gameData.Rows             => location with { Row = location.Row + 1 },
            InputActions.MoveRight when location.Column + 1 < gameData.Columns      => location with { Column = location.Column + 1 },
            _                                                                       => location 
        };

        return location = desiredLocation;
    }

    private void CheckForArrowMove()
    {
        if (ArrowLocation is null) return;
        Location copyLocation = (Location)ArrowLocation;
        ArrowLocation = _inputAction switch
        {
            InputActions.MoveUp when copyLocation.Row - 1 >= 0 => copyLocation with { Row = copyLocation.Row - 1 },
            InputActions.MoveLeft when copyLocation.Column - 1 >= 0 => copyLocation with { Column = copyLocation.Column - 1 },
            InputActions.MoveDown when copyLocation.Row + 1 < gameData.Rows => copyLocation with { Row = copyLocation.Row + 1 },
            InputActions.MoveRight when copyLocation.Column + 1 < gameData.Columns => copyLocation with { Column = copyLocation.Column + 1 },
            _ => Location
        };

        ArrowLocation = copyLocation;
    }
    
    private enum InputActions { UnAccounted, MoveUp, MoveDown, MoveLeft, MoveRight, Repair, Shoot, Menu  }
}