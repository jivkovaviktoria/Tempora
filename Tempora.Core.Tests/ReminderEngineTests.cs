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

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 6,   // Monday
            hour: 9,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar);

        var expected = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 6,
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenWeeklyRuleAndTimeHasPassed_ReturnsNextWeek()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 6,   // Monday
            hour: 11,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar);

        var expected = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 13,  // Next Monday
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenTodayIsNotScheduledDay_ReturnsNextScheduledDay()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 7,   // Tuesday
            hour: 9,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar);

        var expected = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 13,  // Next Monday
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    [Fact]
    public void CalculateNext_WhenMultipleWeekdays_ReturnsNearestUpcomingDay()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 7,   // Tuesday
            hour: 9,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar);

        var expected = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 8,   // Wednesday
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    /// <summary>
    /// Verifies that when a scheduled day falls on a weekend and weekends are excluded,
    /// the execution is moved to the next business day.
    /// </summary>
    [Fact]
    public void CalculateNext_WhenScheduledDayIsWeekendAndWeekendsExcluded_SkipsToMonday()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Saturday)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar
            .Default()
            .ExcludeWeekends();

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 3,   // Friday
            hour: 9,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero);

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar);

        var expected = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 6,   // Monday
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

}
