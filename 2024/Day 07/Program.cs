using Day_07;

var input = File.ReadAllLines("input.txt");

var equations = input
    .Select(line =>
    {
        var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var result = long.Parse(parts[0]);
        var operands = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse);

        return new Equation(result, operands);
    }).ToArray();

long calibrationResult = 0;

foreach (var equation in equations)
{
    if (equation.IsSolvable())
    {
        equation.Display(equation.SolvingOperators);
        calibrationResult += equation.Result;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        equation.Display();
        Console.ResetColor();
    }
}


Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine($"Calibration result: {calibrationResult}");
Console.ResetColor();
