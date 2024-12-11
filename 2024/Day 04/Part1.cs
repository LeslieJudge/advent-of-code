namespace Day_04;

/// <summary>
/// Finds all occurences of XMAS as in Part 1 of the puzzle
/// </summary>
internal static class Part1
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
                if (input[y][x] is 'X' or 'x')
                {
                    possibleStartCoordinates.Add((x, y));
                }
            }
        }

        var allWords = possibleStartCoordinates
            .SelectMany(coords => GetAllWords(coords.x, coords.y))
            .ToArray();

        var xmasCount = allWords
            .Count(word => string.Equals("xmas", word, StringComparison.OrdinalIgnoreCase));

        return xmasCount;

        IEnumerable<string> GetAllWords(int x, int y)
        {
            var words = new List<(bool valid, string word)>
            {
                East(x, y),
                SouthEast(x, y),
                South(x, y),
                SouthWest(x, y),
                West(x, y),
                NorthWest(x, y),
                North(x, y),
                NorthEast(x, y)
            };

            return words
                .Where(word => word.valid)
                .Select(word => word.word);
        }

        (bool valid, string word) East(int x, int y)
        {
            if (x > width - 4)
            {
                return (false, string.Empty);
            }

            return (true, input[y].Substring(x, 4));
        }

        (bool valid, string word) SouthEast(int x, int y)
        {
            if (x > width - 4 || y > height - 4)
            {
                return (false, string.Empty);
            }

            var chars = new List<char>();

            for (var l = 0; l < 4; l++)
            {
                chars.Add(input[y + l][x + l]);
            }

            return (true, new string(chars.ToArray()));
        }

        (bool valid, string word) South(int x, int y)
        {
            if (y > height - 4)
            {
                return (false, string.Empty);
            }

            return (true, new string(input.Skip(y).Take(4).Select(line => line[x]).ToArray()));
        }

        (bool valid, string word) SouthWest(int x, int y)
        {
            if (x < 3 || y > height - 4)
            {
                return (false, string.Empty);
            }

            var chars = new List<char>();

            for (var l = 0; l < 4; l++)
            {
                chars.Add(input[y + l][x - l]);
            }

            return (true, new string(chars.ToArray()));
        }

        (bool valid, string word) West(int x, int y)
        {
            if (x < 3)
            {
                return (false, string.Empty);
            }
            return (true, new string(input[y].Substring(x - 3, 4).Reverse().ToArray()));
        }

        (bool valid, string word) NorthWest(int x, int y)
        {
            if (x < 3 || y < 3)
            {
                return (false, string.Empty);
            }

            var chars = new List<char>();

            for (var l = 0; l < 4; l++)
            {
                chars.Add(input[y - l][x - l]);
            }

            return (true, new string(chars.ToArray()));
        }

        (bool valid, string word) North(int x, int y)
        {
            if (y < 3)
            {
                return (false, string.Empty);
            }

            return (true, new string(input.Skip(y - 3).Take(4).Select(line => line[x]).Reverse().ToArray()));
        }

        (bool valid, string word) NorthEast(int x, int y)
        {
            if (x > width - 4 || y < 3)
            {
                return (false, string.Empty);
            }

            var chars = new List<char>();

            for (var l = 0; l < 4; l++)
            {
                chars.Add(input[y - l][x + l]);
            }

            return (true, new string(chars.ToArray()));
        }
    }
}
