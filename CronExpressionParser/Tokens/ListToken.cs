namespace CronExpressionParser.Tokens;

public class ListToken  
    : IToken
{
    private readonly List<int> _rawInput;

    public ListToken(List<int> rawInput)
    {
        _rawInput = rawInput;
    }

    public IReadOnlyList<int> GetValues(int min, int max)
    {
        //add each item that's comma separated
        _rawInput.Sort();

        var list = new List<int>();
        foreach (var item in _rawInput)
        {
            if (item < min || item > max)
            {
                throw new FormatException("Invalid comma separated list");
            }

            list.Add(item);
        }

        return list;
    }
}