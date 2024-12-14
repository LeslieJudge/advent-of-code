namespace Day_08;

internal sealed class Map
{
    private readonly char[] _cells;
    private readonly Dictionary<char, char[]> _antinodes = [];

    public int Width { get; }
    public int Height { get; }
    public char[] Frequencies { get; }

    private Map(char[] cells, int width, int height)
    {
        (_cells, Width, Height) = (cells, width, height);
        
        Frequencies = FindFrequencies();
        FindAntinodes();
    }

    public static Map Load(string path)
    {
        var lines = File.ReadAllLines(path);
        var width = lines[0].Length;
        var index = 0;
        var cells = new char[width * lines.Length];

        foreach (var line in lines)
        {
            line.AsSpan().CopyTo(cells.AsSpan(index, width));
            index += width;
        }

        return new Map(cells, width, lines.Length);
    }

    public void Display()
    {
        var map = string.Join("\r\n", _cells.Chunk(Width).Select(x => new string(x)));

        Console.Clear();
        Console.WriteLine(map);
    }

    public void Display(bool withAntinodes)
    {
        Console.ResetColor();
        Display();
        
        if (!withAntinodes)
        {
            return;
        }

        var combinedNodeMap = CombineAllFrequencyMaps();

        for (var i = 0; i < combinedNodeMap.Length; i++)
        {
            if (combinedNodeMap[i] is '#')
            {
                var coords = CoordsFromIndex(i);

                Console.SetCursorPosition(coords.x, coords.y);
                Console.ForegroundColor = ConsoleColor.Yellow;

                var marker = Cell(coords.x, coords.y) == '.' ? '#' : Cell(coords.x, coords.y);

                Console.Write(marker);
            }
        }

        Console.SetCursorPosition(0, Height - 1);
        Console.ResetColor();
        Console.WriteLine();
    }

    public void Display(char frequency)
    {
        Console.ResetColor();

        if (!_antinodes.TryGetValue(frequency, out var nodeMap))
        {
            if (frequency is not '*')
            {
                Display();
                return;
            }
                
            nodeMap = CombineAllFrequencyMaps();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Display();

        for (var i = 0; i < nodeMap.Length; i++)
        {
            var coords = CoordsFromIndex(i);
            
            if (nodeMap[i] is '#')
            {

                Console.SetCursorPosition(coords.x, coords.y);
                Console.ForegroundColor = ConsoleColor.Yellow;

                var marker = Cell(coords.x, coords.y) == '.' ? '#' : Cell(coords.x, coords.y);

                Console.Write(marker);
            }

            if (Cell(coords.x, coords.y) == frequency)
            {
                Console.SetCursorPosition(coords.x, coords.y);
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.Write(Cell(coords.x, coords.y));
            }
        }

        Console.SetCursorPosition(0, Height - 1);
        Console.ResetColor();
        Console.WriteLine();
    }

    public int GetAntinodeCount() => CombineAllFrequencyMaps().Count(x => x is '#');

    private char[] FindFrequencies() => _cells.Where(cell => cell is not '.').Distinct().ToArray();

    private void FindAntinodes()
    {
        foreach (var f in Frequencies)
        {
            var nodeMap = new char[Width * Height];

            var antinodes = MakePairs(FindAntennae(f))
                .Select(CalculateAntiNodes)
                .ToArray();

            foreach (var (a, b) in antinodes)
            {
                MarkAntinode(a.x, a.y, nodeMap);
                MarkAntinode(b.x, b.y, nodeMap);
            }

            _antinodes.Add(f, nodeMap);
        }
    }

    private char[] CombineAllFrequencyMaps()
    {
        var result = new char[Width * Height];

        foreach (var nodeMap in _antinodes.Values)
        {
            for (var i = 0; i < nodeMap.Length; i++)
            {
                if (nodeMap[i] is '#')
                {
                    result[i] = '#';
                }
            }
        }

        return result;
    }

    private IEnumerable<(int x, int y)> FindAntennae(char frequency)
    {
        for (var i = 0; i < _cells.Length; i++)
        {
            if (_cells[i] == frequency)
            {
                yield return CoordsFromIndex(i);
            }
        }
    }

    private IEnumerable<((int x, int y) a, (int x, int y) b)> MakePairs(IEnumerable<(int x, int y)> antennae)
    {
        foreach (var a in antennae)
        {
            foreach (var b in antennae)
            {
                if (a != b)
                {
                    yield return (a, b);
                }
            }
        }
    }

    private ((int x, int y) a, (int x, int y) b) CalculateAntiNodes(((int x, int y) a, (int x, int y) b) antennae)
    {
        var xDistance = antennae.a.x - antennae.b.x;
        var yDistance = antennae.a.y - antennae.b.y;

        var antinodeA = (antennae.a.x + xDistance, antennae.a.y + yDistance);
        var antinodeB = (antennae.b.x - xDistance, antennae.b.y - yDistance);

        return (antinodeA, antinodeB);
    }

    private bool IsOutOfBounds(int x, int y) => x < 0 || Width <= x || y < 0 || Height <= y;

    private char Cell(int x, int y) => IsOutOfBounds(x, y) ? '\0' : _cells[y * Width + x];

    private (int x, int y) CoordsFromIndex(int index) => (index % Width, index / Width);

    private void MarkAntinode(int x, int y, char[] nodeMap)
    {
        if (IsOutOfBounds(x, y))
        {
            return;
        }

        nodeMap[y * Width + x] = '#';
    }
}
