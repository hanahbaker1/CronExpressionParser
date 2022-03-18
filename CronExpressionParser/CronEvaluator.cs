using CronExpressionParser.Tokens;

namespace CronExpressionParser;

public class CronEvaluator
{
    public static IReadOnlyList<int> Evaluate(string token, int min, int max)
    {
        ArgumentNullException.ThrowIfNull(token);
        return ParseToken(token).GetValues(min, max);
    }

    private static IToken ParseToken(string token)
    {
        if (token == "*")
        {
            return new AnyToken();
        }

        if (token.Contains("/"))
        {
            return ParseStepToken(token);
        }

        if (token.Contains("-"))
        {
            return ParseRangeToken(token);
        }

        if (token.Contains(","))
        {
            return new ListToken(token.Split(new[] {','}).Select(t => int.Parse(t)).ToList());
        }

        var value = int.Parse(token);
        return new RangeToken(value, value);
    }

    private static IToken ParseStepToken(string token)
    {
        var strings = token.Split('/');
        if (strings.Length != 2)
        {
            throw new FormatException("Invalid step token");
        }

        var range = ParseStepTokenRange(strings[0]);
        var increment = int.Parse(strings[1]);
        return new StepToken(range, increment);
    }

    private static IToken ParseStepTokenRange(string token)
    {
        if (token == "*")
        {
            return new AnyToken();
        }

        if (token.Contains("-"))
        {
            return ParseRangeToken(token);
        }

        throw new FormatException("Invalid step token range");
    }

    private static IToken ParseRangeToken(string token)
    {
        var strings = token.Split('-');

        if (strings.Length != 2)
        {
            throw new FormatException("Invalid range token");
        }

        return new RangeToken(int.Parse(strings[0]), int.Parse(strings[1]));
    }
}