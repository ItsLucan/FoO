namespace The_Fountain_of_Objects.Scripts;

public class Player(GameData gameData)
{
    public Location Location { get; private set; } = gameData.EntranceLocation;
    public int Arrows { get; private set; } = 5;
    private InputActions _inputAction;

    public void Teleport()
    {
        Location = gameData.GetRandomLocation();
    }

    public void Shoot()
    {
        Arrows--;
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
        return _inputAction == InputActions.Repair;
    }
    
    private void CheckForMove()
    {
        Location desiredLocation = _inputAction switch
        {
            InputActions.MoveUp when Location.Row - 1 >= 0                          => Location with { Row = Location.Row - 1 },
            InputActions.MoveLeft when Location.Column - 1 >= 0                     => Location with { Column = Location.Column - 1 },
            InputActions.MoveDown when Location.Row + 1 < gameData.Rows             => Location with { Row = Location.Row + 1 },
            InputActions.MoveRight when Location.Column + 1 < gameData.Columns      => Location with { Column = Location.Column + 1 },
            _                                                                       => Location
        };

        if (Location != desiredLocation) Location = desiredLocation;
    }
    
    private enum InputActions { MoveUp, MoveDown, MoveLeft, MoveRight, Repair, UnAccounted }
}