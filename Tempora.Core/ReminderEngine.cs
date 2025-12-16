namespace Tempora.Core
{
    /// <summary>
    /// Provides functionality for calculating reminder execution times
    /// based on scheduling rules and business calendars.
    /// </summary>
    public static class ReminderEngine
    {
        /// <summary>
        /// Calculates the next valid execution time for a reminder rule.
        /// </summary>
        /// <param name="rule">The reminder rule defining recurrence and timing.</param>
        /// <param name="lastExecution">
        /// The most recent execution time, or <c>null</c> if the reminder has not run yet.
        /// </param>
        /// <param name="now">The current point in time used as the calculation reference.</param>
        /// <param name="calendar">The business calendar used to validate execution dates.</param>
        /// <param name="missedExecutionPolicy">
        /// Specifies how missed executions should be handled.
        /// </param>
        /// <returns>The next valid execution time.</returns>
        public static DateTimeOffset CalculateNext(
            ReminderRule rule,
            DateTimeOffset? lastExecution,
            DateTimeOffset now,
            BusinessCalendar calendar,
            MissedExecutionPolicy missedExecutionPolicy = MissedExecutionPolicy.RunLastOnly)
        {
            if (rule.DaysOfWeek is null || rule.DaysOfWeek.Count == 0)
            {
                throw new InvalidOperationException("Weekly rule must define at least one weekday.");
            }

            var nowInZone = TimeZoneInfo.ConvertTime(now, rule.TimeZone);
            var startDate = DateOnly.FromDateTime(nowInZone.DateTime);

            var candidates = new List<DateTimeOffset>();

            for (var i = 0; i <= 7; i++)
            {
                var date = startDate.AddDays(i);

                if (!rule.DaysOfWeek.Contains(date.DayOfWeek))
                {
                    continue;
                }

                var executionDate = calendar.AdjustToBusinessDay(date);

                var localDateTime = new DateTime(
                    executionDate.Year,
                    executionDate.Month,
                    executionDate.Day,
                    rule.TimeOfDay.Hour,
                    rule.TimeOfDay.Minute,
                    0,
                    DateTimeKind.Unspecified
                );

                var offset = rule.TimeZone.GetUtcOffset(localDateTime);
                var candidate = new DateTimeOffset(localDateTime, offset);

                if (candidate > nowInZone)
                {
                    candidates.Add(candidate);
                }
            }


            if (candidates.Count == 0)
            {
                throw new InvalidOperationException("Unable to calculate next execution.");
            }

            return candidates.Min();
        }

        public static IReadOnlyList<DateTimeOffset> CalculateNextOccurrences(
            ReminderRule rule,
            DateTimeOffset now,
            BusinessCalendar calendar,
            int count,
            MissedExecutionPolicy missedExecutionPolicy = MissedExecutionPolicy.RunLastOnly)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            var results = new List<DateTimeOffset>(count);
            var currentNow = now;

            for (var i = 0; i < count; i++)
            {
                var next = CalculateNext(rule, lastExecution: null, currentNow, calendar, missedExecutionPolicy);

                results.Add(next);
                currentNow = next;
            }

            return results;
        }

    }
}
