using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace CronExpressionParser.Tests;

public class CronExpressionTests
{
    [TestCase("wsdfnjdsnghosrd", Description = "not a cron")]
    [TestCase("* * * * xcvx /sh", Description = "invalid token")]
    [TestCase("* * * * /sh", Description = "not enough tokens")]
    [TestCase("* * * * * * /sh", Description = "too many tokens")]
    [TestCase("2/2/2 * * * * /sh", Description = "invalid step argument")]
    [TestCase("2-3-4 * * * * /sh", Description = "invalid range bounds")]
    [TestCase("2/2 * * * * /sh", Description = "non standard argument")]
    [TestCase("* * 5-3 * * /sh", Description = "min value is greater than max value")]
    [TestCase("60 * * * * /sh", Description = "minutes max is 59")]
    [TestCase("* 24 * * * /sh", Description = "minutes max is 24")]
    [TestCase("* * 0-3 * * /sh", Description = "day of month starts at 1")]
    [TestCase("* * * 0/4 * /sh", Description = "months start at 1")]
    [TestCase("* * * * 0-7 /sh", Description = "days of week is from 0 to 6")]
    public void TryParse_RejectsInvalidStringExpression(string expression)
    {
        // Act
        var result = CronExpression.TryParse(expression, out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeFalse();
        value.Should().BeNull();
    }

    public static object[] ValidMinutes()
    {
        return new object[]
        {
            new object[] {"*", Enumerable.Range(0, 60).ToArray()},
            new object[] {"2-5", new[] {2, 3, 4, 5}},
            new object[] {"2,3,4", new[] {2, 3, 4}},
            new object[] {"*/20", new[] {0, 20, 40}},
            new object[] {"5-59/30", new[] {5, 35}}
        };
    }


    [TestCaseSource(typeof(CronExpressionTests), nameof(ValidMinutes))]
    public void TryParse_Parses_ValidMinutes(string expression, int[] expected)
    {
        // Act
        var result = CronExpression.TryParse($"{expression} * * * * /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.Minutes.Should().BeEquivalentTo(expected);
    }

    public static object[] ValidHours()
    {
        return new object[]
        {
            new object[] {"*", Enumerable.Range(0, 24).ToArray()},
            new object[] {"2-5", new[] {2, 3, 4, 5}},
            new object[] {"2,3,4", new[] {2, 3, 4}},
            new object[] {"*/6", new[] {0, 6, 12, 18}},
            new object[] {"10-20/5", new[] {10, 15, 20  }}
        };
    }

    [TestCaseSource(typeof(CronExpressionTests), nameof(ValidHours))]
    public void TryParse_Parses_ValidHours(string expression, int[] expected)
    {
        // Act
        var result = CronExpression.TryParse($"* {expression} * * * /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.Hours.Should().BeEquivalentTo(expected);
    }

    public static object[] ValidDaysOfMonth()
    {
        return new object[]
        {
            new object[] {"*", Enumerable.Range(1, 31).ToArray()},
            new object[] {"2-5", new[] {2, 3, 4, 5}},
            new object[] {"2,3,4", new[] {2, 3, 4}},
            new object[] {"*/6", new[] { 1, 7, 13, 19, 25, 31 } },
            new object[] {"10-31/5", new[] {10, 15, 20 ,25, 30 }}
        };
    }

    [TestCaseSource(typeof(CronExpressionTests), nameof(ValidDaysOfMonth))]
    public void TryParse_Parses_ValidDaysOfMonth(string expression, int[] expected)
    {
        // Act
        var result = CronExpression.TryParse($"* * {expression} * * /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.DaysOfMonth.Should().BeEquivalentTo(expected);
    }

    public static object[] ValidMonth()
    {
        return new object[]
        {
            new object[] {"*", Enumerable.Range(1, 12).ToArray()},
            new object[] {"2-5", new[] {2, 3, 4, 5}},
            new object[] {"2,3,4", new[] {2, 3, 4}},
            new object[] {"*/4", new[] {1, 5, 9}},
            new object[] {"2-12/2", new[] {2,4,6,8,10,12 }}
        };
    }

    [TestCaseSource(typeof(CronExpressionTests), nameof(ValidMonth))]
    public void TryParse_Parses_ValidMonth(string expression, int[] expected)
    {
        // Act
        var result = CronExpression.TryParse($"* * * {expression} * /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.Months.Should().BeEquivalentTo(expected);
    }

    public static object[] ValidDaysOfWeek()
    {
        return new object[]
        {
            new object[] {"1", new []{1}},
            new object[] {"*", Enumerable.Range(0, 7).ToArray()},
            new object[] {"2-5", new[] {2, 3, 4, 5}},
            new object[] {"2,3,4", new[] {2, 3, 4}},
            new object[] {"*/2", new[] {0, 2, 4, 6}},
            new object[] {"1-6/3", new[] {1, 4 }}
        };
    }

    [TestCaseSource(typeof(CronExpressionTests), nameof(ValidDaysOfWeek))]
    public void TryParse_Parses_ValidDaysOfWeek(string expression, int[] expected)
    {
        // Act
        var result = CronExpression.TryParse($"* * * * {expression} /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.DaysOfWeek.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TryParse_Parses_Command()
    {
        // Act
        var result = CronExpression.TryParse($"* * * * * /sh", out var value);

        // Assert
        using var _ = new AssertionScope();
        result.Should().BeTrue();
        value!.Command.Should().BeEquivalentTo("/sh");
    }

    [Test]
    public void ToString_StringifiesResponse()
    {
        // Arrange
        var expression = new CronExpression()
        {
            DaysOfWeek = new[] { 1, 2, 3 },
            Command = "/usr/bin/find"
        };

        // Act
        var result = expression.ToString();

        // Assert
        result.Should().Be(@"----------------------------
minute         
hour           
day of month   
month          
day of week    1 2 3
command        /usr/bin/find
----------------------------");
    }
}