using Day_06;

var map = Map.Load("input.txt");
var guardIsInTheArea = true;
var autoStep = false;

Console.Clear();

while (guardIsInTheArea)
{
    if (!autoStep)
    {
        map.PrintToConsole();

        var input = Console.ReadKey(intercept: true);

        if (input.Key is ConsoleKey.A)
        {
            autoStep = true;
        }
    }

    guardIsInTheArea = map.Step();
}

map.PrintToConsole();

Console.WriteLine();
Console.WriteLine();
Console.WriteLine($"Visited positions: {map.GetVisitedPositionCount()}");
