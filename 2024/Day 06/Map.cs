namespace Day_06;

internal sealed class Map
{
    private readonly char[] _cells;
    private readonly int _width;
    private readonly int _height;
    private char _guard;
    private int _guardX;
    private int _guardY;
    private int _directionX;
    private int _directionY;

    private Map(char[] cells, int width, int height)
    {
        (_cells, _width, _height) = (cells, width, width);
        (_guardX, _guardY) = FindGuard();
        (_directionX, _directionY) = (0, -1);
        _guard = '^';
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

    public bool Step()
    {
        MarkAsVisited(_guardX, _guardY);

        var x = _guardX + _directionX;
        var y = _guardY + _directionY;

        if (IsOccupied(x, y))
        {
            Turn();

            x = _guardX + _directionX;
            y = _guardY + _directionY;
        }

        _guardX = x;
        _guardY = y;

        if (GuardLeft())
        {
            return false;
        }

        _cells[_guardY * _width + _guardX] = _guard;

        return true;
    }

    private void Turn()
    {
        switch (_guard)
        {
            case '^':
                _directionX = 1;
                _directionY = 0;
                _guard = '>';
                break;
            case '>':
                _directionX = 0;
                _directionY = 1;
                _guard = 'v';
                break;
            case 'v':
                _directionX = -1;
                _directionY = 0;
                _guard = '<';
                break;
            case '<':
                _directionX = 0;
                _directionY = -1;
                _guard = '^';
                break;
            default:
                break;
        }
    }

    private (int x, int y) FindGuard()
    {
        var index = _cells.AsSpan().IndexOf('^');
        var y = index / _width;
        var x = index % _width;

        return (x, y);
    }

    private bool IsOccupied(int x, int y)
    {
        if (x >= _width || y >= _height)
        {
            return false;
        }

        return _cells[y * _width + x] is '#';
    }

    private void MarkAsVisited(int x, int y) => _cells[y * _width + x] = 'X';

    private bool GuardLeft() => _guardX >= _width || _guardY >= _height;

    public void PrintToConsole()
    {
        Console.CursorTop = 0;
        Console.CursorLeft = 0;

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                Console.ForegroundColor = _cells[y * _width + x] switch
                {
                    '^' or '>' or 'v' or '<' => ConsoleColor.Yellow,
                    '#' => ConsoleColor.Magenta,
                    'X' => ConsoleColor.DarkCyan,
                    _ => ConsoleColor.Gray,
                };

                Console.Write(_cells[y * _width + x]);
            }

            if (Console.CursorLeft != 0)
            {
                Console.WriteLine();
            }
        }
    }

    public int GetVisitedPositionCount() => _cells.Count(x => x is 'X');
}
