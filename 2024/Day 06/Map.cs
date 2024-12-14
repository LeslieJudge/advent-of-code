using System.Diagnostics;

namespace Day_06;

internal sealed class Map
{
    private readonly char[] _cells;
    private char[] _backup;
    private char _guard;
    private int _guardX;
    private int _guardY;
    private int _directionX;
    private int _directionY;
    
    public int Width { get; }
    public int Height { get; }
    public bool ShowProgress { get; set; }

    private Map(char[] cells, int width, int height)
    {
        (_cells, Width, Height) = (cells, width, height);
        (_guardX, _guardY) = FindGuard();
        (_directionX, _directionY) = (0, -1);
        _guard = '^';

        _backup = new char[Width * Height];
        _cells.CopyTo(_backup, 0);
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

    public void Reset()
    {
        _backup.CopyTo(_cells, 0);
        (_guardX, _guardY) = FindGuard();
        (_directionX, _directionY) = (0, -1);
        _guard = '^';
    }

    public bool FindLoop()
    {
        StepResult stepResult;

        do
        {
            stepResult = Step();
        } while (stepResult is StepResult.OK);

        return stepResult is StepResult.LoopFound;
    }

    private bool NextStepWouldBeATurn(int x, int y)
    {
        x += _directionX;
        y += _directionY;

        return IsOccupied(x, y);
    }

    public StepResult Step()
    {
        var x = _guardX + _directionX;
        var y = _guardY + _directionY;
        var marker = 'X';

        var turnCount = 0;

        while (IsOccupied(x, y))
        {
            Turn();
            turnCount++;

            x = _guardX + _directionX;
            y = _guardY + _directionY;

            marker = 'x';
        }

        if (IsTurningPosition(x, y) && NextStepWouldBeATurn(x, y))
        {
            return StepResult.LoopFound;
        }

        MarkAsVisited(_guardX, _guardY, marker);

        if (ShowProgress)
        {
            DrawCell(_guardX, _guardY, withColor: true);
        }

        _guardX = x;
        _guardY = y;

        if (GuardLeft())
        {
            return StepResult.GuardLeft;
        }

        _cells[_guardY * Width + _guardX] = _guard;

        if (ShowProgress)
        {
            DrawCell(_guardX, _guardY, withColor: true);
        }

        return StepResult.OK;
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
        var y = index / Width;
        var x = index % Width;

        return (x, y);
    }

    public void PlaceObstacle(int x, int y)
    {
        _cells[y * Width + x] = 'O';

        if (ShowProgress)
        {
            DrawCell(x, y, withColor: true);
        }
    }

    public bool IsOccupied(int x, int y)
    {
        if (IsOutOfBounds(x, y))
        {
            return false;
        }

        return _cells[y * Width + x] is '#' or 'O' or '^' or '>' or 'v' or '<';
    }

    private void MarkAsVisited(int x, int y, char mark) => _cells[y * Width + x] = mark;

    private bool IsTurningPosition(int x, int y)
    {
        if (IsOutOfBounds(x, y))
        {
            return false;
        }

        return _cells[y * Width + x] is 'x';
    }

    private bool IsOutOfBounds(int x, int y) => x < 0 || Width <= x || y < 0 || Height <= y;

    private bool GuardLeft() => IsOutOfBounds(_guardX, _guardY);

    private char Cell(int x, int y) => _cells[y * Width + x];

    public void Display(bool withColors)
    {
        Console.SetCursorPosition(0, 0);

        if (withColors)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    DrawCell(x, y, withColors);
                }

                if (Console.CursorLeft != 0)
                {
                    Console.WriteLine();
                }
            }

            Console.ResetColor();
        }
        else
        {
            var map = string.Join("\r\n", _cells.Chunk(Width).Select(x => new string(x)));
                
            Console.ResetColor();
            Console.WriteLine(map);
        }
    }

    public int GetVisitedPositionCount() => _cells.Count(x => x is 'X' or 'x');

    private void SetColor(char cell)
    {
        Console.ForegroundColor = cell switch
        {
            '^' or '>' or 'v' or '<' => ConsoleColor.Yellow,
            '#' => ConsoleColor.Magenta,
            'O' => ConsoleColor.Green,
            'X' or 'x' => ConsoleColor.DarkCyan,
            _ => ConsoleColor.Gray,
        };
    }
    private void DrawCell(int x, int y, bool withColor) => DrawCell(x, y, Cell(x, y), withColor);

    private void DrawCell(int x, int y, char cell, bool withColor)
    {
        if (withColor)
        {
            SetColor(cell);
        }

        Console.SetCursorPosition(x, y);
        Console.Write(cell);
        Console.ResetColor();
    }

    public enum StepResult
    {
        OK,
        GuardLeft,
        LoopFound
    }
}
