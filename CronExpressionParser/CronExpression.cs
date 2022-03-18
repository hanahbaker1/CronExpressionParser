using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace CronExpressionParser;

public class CronExpression
{
    public IReadOnlyList<int> Minutes { get; set; } = Array.Empty<int>();

    public IReadOnlyList<int> Hours { get; set; } = Array.Empty<int>();

    public IReadOnlyList<int> DaysOfMonth { get; set; } = Array.Empty<int>();

    public IReadOnlyList<int> Months { get; set; } = Array.Empty<int>();

    public IReadOnlyList<int> DaysOfWeek { get; set; } = Array.Empty<int>();

    public string Command { get; set; } = null!;

    public static bool TryParse(string cronExpression, [NotNullWhen(true)] out CronExpression? result)
    {
        try
        {
            var delimiterChars = new[] {' '};
            var tokens = cronExpression.Split(delimiterChars);
            if (tokens.Length != 6)
            {
                result = null;
                return false;
            }

            result = new CronExpression
            {
                Minutes = CronEvaluator.Evaluate(tokens[0], 0, 59),
                Hours = CronEvaluator.Evaluate(tokens[1], 0, 23),
                DaysOfMonth = CronEvaluator.Evaluate(tokens[2], 1, 31),
                Months = CronEvaluator.Evaluate(tokens[3], 1, 12),
                DaysOfWeek = CronEvaluator.Evaluate(tokens[4], 0, 6),
                Command = tokens[5],
            };
            return true;
        }
        catch (FormatException)
        {
            result = null;
            return false;
        }
    }

    public override string ToString()
    {
        var minutes = GetValues(Minutes);
        var hours = GetValues(Hours);
        var daysOfMonth = GetValues(DaysOfMonth);
        var months = GetValues(Months);
        var daysOfWeek = GetValues(DaysOfWeek);

        string[] displayItem = { "minute", "hour", "day of month", "month", "day of week", "command" };
        string[] displayValues = { minutes, hours, daysOfMonth, months, daysOfWeek, Command };

        var lines = new List<string>();
        
        for (int i = 0; i < displayItem.Length; i++)
        {
            lines.Add($"{displayItem[i],-14} {displayValues[i]}");
        }

        var tableBorder = new string('-', lines.Max(l => l.Length));
        return string.Join(Environment.NewLine, lines.Prepend(tableBorder).Append(tableBorder));
    }

    private static string GetValues(IEnumerable<int> values)
    {
        return string.Join(" ", values.OrderBy(x => x));
    }
}