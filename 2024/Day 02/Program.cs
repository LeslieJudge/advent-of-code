var input = File.ReadAllLines("input.txt")
    .Select(line => line
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(value => int.Parse(value))
        .ToArray())
    .ToArray();

var safeCount = input.Count(IsSafe);
var safeWithDampenerCount = input.Count(IsSafeWithDampener);

Console.WriteLine($"Safe reports: {safeCount}");
Console.WriteLine($"Safe reports with dampener: {safeWithDampenerCount}");

static bool IsSafe(int[] levels)
{
    if (levels.Length == 1)
    {
        return true;
    }

    var sign = Math.Sign(levels[0] - levels[1]);

    return IsSafeWithSign(levels, sign);
}

static bool IsSafeWithSign(int[] levels, int sign)
{
    var lefts = levels[..^1];
    var rights = levels[1..];

    return lefts.Zip(rights).All(pair => IsSafePair(pair.First, pair.Second, sign));
}

static bool IsSafePair(int left, int right, int sign)
{
    return Math.Abs(left - right) is > 0 and < 4
        && Math.Sign(left - right) == sign;
}

static bool IsSafeWithDampener(int[] levels)
{
    var variations = CreateVariations(levels);

    return variations.Any(IsSafe);
}

static int[][] CreateVariations(int[] levels)
{
    var variations = new List<int[]>();

    for (var i = 0; i < levels.Length; i++)
    {
        var variation = new int[levels.Length - 1];

        Array.Copy(levels, variation, i);
        Array.Copy(levels, i + 1, variation, i, levels.Length - i - 1);

        variations.Add(variation);
    }

    return variations.ToArray();
}