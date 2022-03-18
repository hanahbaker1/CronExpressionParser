namespace CronExpressionParser.Tokens;

public interface IToken
{
    IReadOnlyList<int> GetValues(int min, int max);
}