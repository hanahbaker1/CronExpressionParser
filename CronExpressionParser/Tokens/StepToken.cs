namespace CronExpressionParser.Tokens;

public class StepToken
    : IToken
{
    private readonly IToken _range;
    private readonly int _increment;

    public StepToken(IToken range, int increment)
    {
        _range = range;
        _increment = increment;
    }

    public IReadOnlyList<int> GetValues(int min, int max)
    {
        var possibleValues = _range.GetValues(min, max);
        var list = new List<int>();

        for (var i = 0; i < possibleValues.Count; i += _increment)
        {
            list.Add(possibleValues[i]);
        }

        return list;
    }
}