namespace CronExpressionParser.Tokens;

public class AnyToken
    : IToken
{
    public IReadOnlyList<int> GetValues(int min, int max)
    {
        var list = new List<int>();
        for (var i = min; i <= max; i++)
        {
            list.Add(i);
        }

        return list;
    }
}