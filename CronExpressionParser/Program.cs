namespace CronExpressionParser;

public class Program
{
    public static void Main(string[] args)
    {
        var expression = args[0];

        if (CronExpression.TryParse(expression, out var result))
        {
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("Expected [minute] [hour] [day of month] [month] [day of week] [command] but got : " +
                              expression);
        }
    }
}