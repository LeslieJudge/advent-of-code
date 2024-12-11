var input = File.ReadAllLines("input.txt");

var rules = new List<int[]>();
var updates = new List<int[]>();
var separatorLineIndex = 0;

for (var index = 0; index < input.Length; index++)
{
    if (input[index].Length == 0)
    {
        separatorLineIndex = index;
        break;
    }

    rules.Add(ParsePageNumbers(input[index], '|'));
}

for (var index = separatorLineIndex + 1; index < input.Length; index++)
{
    updates.Add(ParsePageNumbers(input[index], ','));
}

var sum = updates
    .Where(IsCorrect)
    .Select(GetMiddlePageNumber)
    .Sum();

Console.WriteLine($"Sum of the middle page numbers of the already correct updates: {sum}");

int[] ParsePageNumbers(string line, char separator) => line
    .Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse)
    .ToArray();

bool IsCorrect(int[] pages)
{
    foreach (var rule in rules)
    {
        if (rule.Any(page => !pages.Contains(page)))
        {
            continue;
        }

        var firstIndex = pages.AsSpan().IndexOf(rule[0]);
        var secondIndex = pages.AsSpan().IndexOf(rule[1]);

        if (firstIndex > secondIndex)
        {
            return false;
        }
    }

    return true;
}

static int GetMiddlePageNumber(int[] pages)
{
    var middleIndex = pages.Length / 2;

    return pages[middleIndex];
}
