using Day_06;

var map = Map.Load("input.txt");
var guardIsInTheArea = true;
var autoStep = false;

Console.SetBufferSize(Math.Max(Console.BufferWidth, map.Width), Math.Max(Console.BufferHeight, map.Height));
Console.Clear();

map.Display(withColors: true);
map.ShowProgress = true;

while (guardIsInTheArea)
{
    if (!autoStep)
    {
        var input = Console.ReadKey(intercept: true);

        if (input.Key is ConsoleKey.A)
        {
            autoStep = true;
        }
    }

    guardIsInTheArea = map.Step() is not Map.StepResult.GuardLeft;
}

map.Display(withColors: true);

Console.WriteLine();
Console.WriteLine();
Console.WriteLine($"Visited positions: {map.GetVisitedPositionCount()}");
Console.ReadKey(intercept: true);
Console.Clear();

map.ShowProgress = false;
var triedPositions = 0;
var loopCount = 0;

if (!map.ShowProgress)
{
    Console.WriteLine("Finding loops...");
}

for (var y = 0; y < map.Height; y++)
{
    for (var x = 0; x < map.Width; x++)
    {
        triedPositions++;
        map.Reset();

        if (map.IsOccupied(x, y))
        {
            continue;
        }

        if (map.ShowProgress)
        {
            map.Display(withColors: false);
        }

        map.PlaceObstacle(x, y);

        if (map.FindLoop())
        {
            loopCount++;
        }

        if (map.ShowProgress)
        {
            Console.SetCursorPosition(0, map.Height + 2);
            Console.WriteLine($"Tried obstacle positions: {triedPositions}");
            Console.WriteLine($"Loops found: {loopCount}");
        }
    }
}

if (!map.ShowProgress)
{
    Console.WriteLine($"Tried obstacle positions: {triedPositions}");
    Console.WriteLine($"Loops found: {loopCount}");
}

