namespace Day_07;

internal sealed class Equation
{
    private readonly char[] _operators = ['+', '*'];
    private readonly int[] _operands;

    public Equation(long result, IEnumerable<int> operands)
    {
        Result = result;
        _operands = operands.ToArray();
    }

    public long Result { get; }
    public IEnumerable<int> Operands => _operands;
    public int OperandCount => _operands.Length;
    public IEnumerable<char> SolvingOperators { get; private set; } = [];

    public bool IsSolvable()
    {
        foreach (var operators in CreateCombinations(OperandCount - 1))
        {
            if (Evaluate(operators) == Result)
            {
                SolvingOperators = operators;

                return true;
            }
        }

        return false;
    }

    private long Evaluate(IEnumerable<char> operators)
    {
        var opArray = operators.ToArray();
        long result = _operands[0];

        for (var i = 1; i < OperandCount; i++)
        {
            switch (opArray[i - 1])
            {
                case '+':
                    result += _operands[i];
                    break;
                case '*':
                    result *= _operands[i];
                    break;
                default:
                    break;
            }
        }

        return result;
    }

    private IEnumerable<IEnumerable<char>> CreateCombinations(int level)
    {
        if (level > 1)
        {
            for (var i = 0; i < _operators.Length; i++)
            {
                var next = CreateCombinations(level - 1);

                foreach (var combo in next)
                {
                    yield return new char[] { _operators[i] }.Concat(combo);
                }
            }
        }
        else
        {
            for (var i = 0; i < _operators.Length; i++)
            {
                yield return new char[] { _operators[i] };
            }
        }
    }

    public void Display()
    {
        Console.WriteLine($"{Result}: {string.Join(" ", Operands)}");
    }

    public void Display(IEnumerable<char> operators)
    {
        var opArray = operators.ToArray();

        Console.Write($"{Result}: {_operands[0]}");

        for (var i = 1; i < OperandCount; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {opArray[i - 1]} ");
            Console.ResetColor();
            Console.Write(_operands[i]);
        }

        Console.WriteLine();
    }
}
