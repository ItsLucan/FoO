namespace The_Fountain_of_Objects.Scripts;


public class Player(GameData gameData)
{
    public Location Location { get; private set; } = gameData.EntranceLocation;
    public Location? ArrowLocation { get; private set; }
    public int Arrows { get; private set; }= 5;
    private InputActions _inputAction;
    
    
    public void ProcessInput()
    {
        _inputAction = GetInput();
        Location = MoveOnInputFrom(Location);
    }
   
    
    public void TryMoveArrow()
    {
        if (ArrowLocation is null) return;
        _inputAction = GetInput();
        ArrowLocation = MoveOnInputFrom((Location)ArrowLocation);
    }
    
 
    private InputActions GetInput()
    {
        InputActions inputAction = Console.ReadKey(true).Key switch
        {
            ConsoleKey.R                          => InputActions.Repair,
            ConsoleKey.Spacebar                   => InputActions.Shoot,
            ConsoleKey.Tab                        => InputActions.Menu,
            ConsoleKey.W or ConsoleKey.UpArrow    => InputActions.MoveUp,
            ConsoleKey.A or ConsoleKey.LeftArrow  => InputActions.MoveLeft,
            ConsoleKey.S or ConsoleKey.DownArrow  => InputActions.MoveDown,
            ConsoleKey.D or ConsoleKey.RightArrow => InputActions.MoveRight,
            ConsoleKey.Escape                     => InputActions.Escape,
            _                                     => InputActions.UnAccounted
        };

        return inputAction;
    }
   
    
    private Location MoveOnInputFrom(Location location)
    {
        Location desiredLocation = _inputAction switch
        {
            InputActions.MoveUp when location.Row - 1 >= 0                          => location with { Row = location.Row - 1 },
            InputActions.MoveLeft when location.Column - 1 >= 0                     => location with { Column = location.Column - 1 },
            InputActions.MoveDown when location.Row + 1 < gameData.Rows             => location with { Row = location.Row + 1 },
            InputActions.MoveRight when location.Column + 1 < gameData.Columns      => location with { Column = location.Column + 1 },
            _                                                                       => location 
        };

        return desiredLocation;
    }
   
    
    public void Teleport() => Location = gameData.GetRandomLocation();
    public bool IsInputtingRepair() => _inputAction == InputActions.Repair;
    public bool IsInputtingShoot() => _inputAction == InputActions.Shoot;
    public bool IsInputtingEscape() => _inputAction == InputActions.Escape;
    public bool IsOpeningMenu() =>  _inputAction == InputActions.Menu;
    public void SubtractArrow() => Arrows--;
    public void SetArrowHere() => ArrowLocation = Location;
    public void ResetArrow() => ArrowLocation = null; 
   
    
    private enum InputActions { UnAccounted, MoveUp, MoveDown, MoveLeft, MoveRight, Repair, Shoot, Menu, Escape }
}