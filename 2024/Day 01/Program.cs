var input = File.ReadAllLines("input.txt")
    .Select(line =>
    {
        var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return (left: int.Parse(data[0]), right: int.Parse(data[1]));
    })
    .ToArray();

var leftList = input.Select(data => data.left).Order().ToArray();
var rightList = input.Select(data => data.right).Order().ToArray();

var distance = leftList.Zip(rightList, (left, right) => Math.Abs(left - right)).Sum();

var leftSet = leftList.ToHashSet();
var rightOccurences = rightList.GroupBy(x => x)
    .ToDictionary(group => group.Key, group => group.Count());

var similarity = rightOccurences
    .Where(right => leftSet.Contains(right.Key))
    .Select(right => right.Key * right.Value)
    .Sum();

Console.WriteLine($"Distance: {distance}");
Console.WriteLine($"Similarity: {similarity}");
