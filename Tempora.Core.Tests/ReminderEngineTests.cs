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

    [Fact]
    public void CalculateNextOccurrences_WeeklyRule_ReturnsNextThreeOccurrences()
    {
        var rule = ReminderRule
            .Weekly(DayOfWeek.Monday)
    /// <summary>
    /// Verifies that when a monthly rule targets a day that has not yet
    /// occurred in the current month, the execution is scheduled
    /// within the same month.
    /// </summary>
    [Fact]
    public void CalculateNext_WhenMonthlyRuleAndDayIsInFuture_ReturnsSameMonth()
    {
        var rule = ReminderRule
            .Monthly(15)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(2025, 1, 6, 9, 0, 0, TimeSpan.Zero);

        var occurrences = ReminderEngine.CalculateNextOccurrences(
            rule,
            now,
            calendar,
            count: 3
        );

        Assert.Equal(3, occurrences.Count);
        Assert.Equal(new DateTimeOffset(2025, 1, 6, 10, 30, 0, TimeSpan.Zero), occurrences[0]);
        Assert.Equal(new DateTimeOffset(2025, 1, 13, 10, 30, 0, TimeSpan.Zero), occurrences[1]);
        Assert.Equal(new DateTimeOffset(2025, 1, 20, 10, 30, 0, TimeSpan.Zero), occurrences[2]);
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
        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 10,
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
            day: 15,
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    /// <summary>
    /// Verifies that when a monthly rule targets a day that has already
    /// passed in the current month, the execution is scheduled
    /// in the next month.
    /// </summary>
    [Fact]
    public void CalculateNext_WhenMonthlyRuleAndDayHasPassed_ReturnsNextMonth()
    {
        var rule = ReminderRule
            .Monthly(15)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(
            year: 2025,
            month: 1,
            day: 20,
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
            month: 2,
            day: 15,
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    /// <summary>
    /// Verifies that when a monthly rule specifies a day that does not exist
    /// in the current month, the execution is rolled forward to the next
    /// month where the day exists.
    /// </summary>
    [Fact]
    public void CalculateNext_WhenMonthlyRuleDayDoesNotExist_RollsForwardToNextValidMonth()
    {
        var rule = ReminderRule
            .Monthly(31)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar.Default();

        var now = new DateTimeOffset(
            year: 2025,
            month: 2,
            day: 1,   // February
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
            month: 3,  // March
            day: 31,
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero);

        Assert.Equal(expected, nextExecution);
    }

    /// <summary>
    /// Verifies that when a monthly execution date falls on a weekend and
    /// weekend exclusion is enabled, the execution is rolled forward to
    /// the next business day.
    /// </summary>
    [Fact]
    public void CalculateNext_WhenMonthlyRuleFallsOnWeekendAndWeekendsExcluded_RollsToNextBusinessDay()
    {
        var rule = ReminderRule
            .Monthly(15)
            .At(10, 30)
            .InTimeZone("UTC");

        var calendar = BusinessCalendar
            .Default()
            .ExcludeWeekends();

        var now = new DateTimeOffset(
            year: 2025,
            month: 3,
            day: 1,
            hour: 9,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var nextExecution = ReminderEngine.CalculateNext(
            rule,
            lastExecution: null,
            now,
            calendar
        );

        var expected = new DateTimeOffset(
            year: 2025,
            month: 3,
            day: 17,  // Monday
            hour: 10,
            minute: 30,
            second: 0,
            offset: TimeSpan.Zero
        );

        Assert.Equal(expected, nextExecution);
    }

}
