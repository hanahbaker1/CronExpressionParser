namespace CronExpressionParser.Tokens;

public class RangeToken
    : IToken
{
    private readonly int _rangeMin;
    private readonly int _rangeMax;

    public RangeToken(int rangeMin, int rangeMax)
    {
        if (rangeMin > rangeMax)
        {
            throw new FormatException("Invalid range: min value must be less than max value");
        }

        _rangeMin = rangeMin;
        _rangeMax = rangeMax;
    }

    public IReadOnlyList<int> GetValues(int min, int max)
    {
        if (_rangeMin < min || _rangeMax > max)
        {
            throw new FormatException("Invalid comma separated list");
        }

        var list = new List<int>();
        for (var i = _rangeMin; i <= _rangeMax; i++)
        {
            list.Add(i);
        }

        return list;
    }
}