namespace Day_04;

internal static class Part2
{
    public static int Count(string[] input)
    {
        var width = input.First().Length;
        var height = input.Length;

        var possibleStartCoordinates = new List<(int x, int y)>();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (input[y][x] is 'A' or 'a')
                {
                    possibleStartCoordinates.Add((x, y));
                }
            }
        }

        var xmasCount = possibleStartCoordinates
            .Count(coords => IsValidCoordinates(coords.x, coords.y));

        return xmasCount;

        bool IsValidCoordinates(int x, int y)
        {
            if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
            {
                return false;
            }

            string[] nwse = [
                new([input[y-1][x-1],input[y][x],input[y+1][x+1]]),
                new([input[y+1][x+1],input[y][x],input[y-1][x-1]]),
            ];

            string[] nesw = [
                new([input[y+1][x-1],input[y][x],input[y-1][x+1]]),
                new([input[y-1][x+1],input[y][x],input[y+1][x-1]]),
            ];

            return nwse.Any(word => string.Equals("mas", word, StringComparison.OrdinalIgnoreCase))
                && nesw.Any(word => string.Equals("mas", word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
