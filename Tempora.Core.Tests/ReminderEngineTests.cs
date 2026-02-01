namespace Tempora.Core.Tests;

public class ReminderEngineTests
{
    [Fact]
    public void CalculateNext_WhenWeeklyRuleAndNoPreviousExecution_ReturnsNextOccurrence()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 6, 9, 0, 0, TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(rule, null, now, calendar);

        Assert.Equal(
            new DateTimeOffset(2025, 1, 6, 10, 30, 0, TimeSpan.Zero),
            nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenWeeklyRuleAndTimeHasPassed_ReturnsNextWeek()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 6, 11, 0, 0, TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(rule, null, now, calendar);

        Assert.Equal(
            new DateTimeOffset(2025, 1, 13, 10, 30, 0, TimeSpan.Zero),
            nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenMultipleWeekdays_ReturnsNearestUpcomingDay()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 7, 9, 0, 0, TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(rule, null, now, calendar);

        Assert.Equal(
            new DateTimeOffset(2025, 1, 8, 10, 30, 0, TimeSpan.Zero),
            nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenMonthlyRuleAndDayIsInFuture_ReturnsSameMonth()
    {
        var rule = ReminderRule
            .Monthly(15)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 6, 9, 0, 0, TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(rule, null, now, calendar);

        Assert.Equal(
            new DateTimeOffset(2025, 1, 15, 10, 30, 0, TimeSpan.Zero),
            nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenMonthlyRuleAndDayHasPassed_ReturnsNextMonth()
    {
        var rule = ReminderRule
            .Monthly(15)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 20, 9, 0, 0, TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(rule, null, now, calendar);

        Assert.Equal(
            new DateTimeOffset(2025, 2, 15, 10, 30, 0, TimeSpan.Zero),
            nextExecution);
    }

    [Fact]
    public void CalculateNextOccurrences_WhenCountIsZero_Throws()
    {
        var rule = ReminderRule.Weekly(DayOfWeek.Monday).At(10, 0);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ReminderEngine.CalculateNextOccurrences(
                rule,
                DateTimeOffset.UtcNow,
                BusinessCalendar.Default(),
                count: 0));
    }

    [Fact]
    public void CalculateNextOccurrences_ReturnsCorrectNumberOfOccurrences()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 6, 9, 0, 0, TimeSpan.Zero);

        var occurrences = ReminderEngine.CalculateNextOccurrences(
            rule,
            now,
            calendar,
            count: 3);

        Assert.Equal(3, occurrences.Count);
        Assert.Equal(new DateTimeOffset(2025, 1, 6, 10, 30, 0, TimeSpan.Zero), occurrences[0]);
        Assert.Equal(new DateTimeOffset(2025, 1, 13, 10, 30, 0, TimeSpan.Zero), occurrences[1]);
        Assert.Equal(new DateTimeOffset(2025, 1, 20, 10, 30, 0, TimeSpan.Zero), occurrences[2]);
    }
}
