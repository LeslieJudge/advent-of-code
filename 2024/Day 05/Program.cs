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

var correctUpdates = updates
    .Where(IsCorrect)
    .ToArray();
var sumOfCorrects = correctUpdates
    .Select(GetMiddlePageNumber)
    .Sum();

var incorrectUpdates = updates
    .Except(correctUpdates)
    .ToArray();
var correctedUpdates = incorrectUpdates
    .Select(MakeCorrection)
    .ToArray();
var sumOfCorrected = correctedUpdates
    .Select(GetMiddlePageNumber)
    .Sum();

Console.WriteLine($"Sum of the middle page numbers of the already correct updates: {sumOfCorrects}");
Console.WriteLine($"Sum of the middle page numbers of the corrected updates: {sumOfCorrected}");

int[] ParsePageNumbers(string line, char separator) => line
    .Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse)
    .ToArray();

bool IsCorrect(int[] pages)
{
    foreach (var rule in rules.Where(rule => IsApplicable(pages, rule)))
    {
        if (!IsCorrectByRule(pages, rule))
        {
            return false;
        }
    }

    return true;
}

bool IsApplicable(int[] pages, int[] rule) => rule.All(page => pages.Contains(page));

bool IsCorrectByRule(int[] pages, int[] rule)
{
    var firstIndex = pages.AsSpan().IndexOf(rule[0]);
    var secondIndex = pages.AsSpan().IndexOf(rule[1]);

    return firstIndex < secondIndex;
}

static int GetMiddlePageNumber(int[] pages)
{
    var middleIndex = pages.Length / 2;

    return pages[middleIndex];
}

int[] MakeCorrection(int[] pages)
{
    var result = new int[pages.Length];
    
    Array.Copy(pages, result, pages.Length);
    
    var modified = false;
    var applicableRules = rules
        .Where(rule => IsApplicable(pages, rule))
        .ToArray();

    do
    {
        modified = false;

        foreach (var rule in applicableRules)
        {
            if (IsCorrectByRule(result, rule))
            {
                continue;
            }
            else
            {
                var firstIndex = result.AsSpan().IndexOf(rule[0]);
                var secondIndex = result.AsSpan().IndexOf(rule[1]);

                result[firstIndex] = rule[1];
                result[secondIndex] = rule[0];
                modified = true;
            }
        }

    } while (modified);


    return result;
}

/*
 * 43|17    [43,17]
 * 43|59    [43,59,17]
 * 43|61    [43,61,59,17]
 * 17|61    [43,17,59,61]
 */