using Day_08;

var map = Map.Load("example.txt");
var frequency = '\0';

while (true) 
{
    map.Display(frequency);

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine($"Antinode count: {map.GetAntinodeCount()}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine($"Available frequencies: {string.Join(", ", map.Frequencies.Order())}");
    Console.WriteLine();
    Console.WriteLine("Press SPACE to display all frequencies at once.");
    Console.WriteLine("Press ESC to exit.");

    var input = Console.ReadKey(intercept: true);

    if (input.Key is ConsoleKey.Escape)
    {
        break;
    }
    else if (input.Key is ConsoleKey.Spacebar)
    {
        frequency = '*';
    }
    else
    {
        frequency = input.KeyChar;
    }
}
