using Day_04;

var input = File.ReadAllLines("input.txt");

var xmasCount = Part1.Count(input);
var x_masCount = Part2.Count(input);

Console.WriteLine($"XMAS count: {xmasCount}");
Console.WriteLine($"X-MAS count: {x_masCount}");

