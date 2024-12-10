using System.Text.RegularExpressions;

var input = File.ReadAllText("input.txt");

var instructionsRegex = new Regex(@"(?<mul>mul\((?<left>\d+),(?<right>\d+)\))|(?<do>do\(\))|(?<dont>don't\(\))", RegexOptions.Compiled);

var instructions = instructionsRegex.Matches(input);
var sum = instructions
    .Where(match => match.Groups["mul"].Success)
    .Select(match => int.Parse(match.Groups["left"].Value) * int.Parse(match.Groups["right"].Value))
    .Sum();

var multiplicationEnabled = true;
var correctedSum = 0;

foreach (Match instruction in instructions)
{
    if (instruction.Groups["do"].Success)
    {
        multiplicationEnabled = true;
    }
    else if (instruction.Groups["dont"].Success)
    {
        multiplicationEnabled = false;
    }
    else if (multiplicationEnabled)
    {
        correctedSum += int.Parse(instruction.Groups["left"].Value) * int.Parse(instruction.Groups["right"].Value);
    }
}

Console.WriteLine($"The result: {sum}");
Console.WriteLine($"The corrected result: {correctedSum}");
