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
            if (rule.DayOfMonth.HasValue)
            {
                return CalculateNextMonthly(rule, now, calendar);
            }

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

        /// <summary>
        /// Calculates the next execution time for a monthly reminder rule.
        /// </summary>
        private static DateTimeOffset CalculateNextMonthly(ReminderRule rule, DateTimeOffset now, BusinessCalendar calendar)
        {
            var nowInZone = TimeZoneInfo.ConvertTime(now, rule.TimeZone);
            var targetDay = rule.DayOfMonth!.Value;

            var startYear = nowInZone.Year;
            var startMonth = nowInZone.Month;

            for (var offsetMonths = 0; offsetMonths < 12; offsetMonths++)
            {
                var candidateMonth = new DateTime(startYear, startMonth, 1).AddMonths(offsetMonths);

                if (!TryCreateLocalDateTime(candidateMonth.Year, candidateMonth.Month, targetDay, rule, out var candidateLocal))
                {
                    continue;
                }

                if (offsetMonths == 0 && candidateLocal <= nowInZone.DateTime)
                {
                    continue;
                }

                //(default: roll forward)
                var executionDate = calendar.AdjustToBusinessDay(DateOnly.FromDateTime(candidateLocal));

                var executionDateTime = new DateTime(
                    executionDate.Year,
                    executionDate.Month,
                    executionDate.Day,
                    rule.TimeOfDay.Hour,
                    rule.TimeOfDay.Minute,
                    0,
                    DateTimeKind.Unspecified
                );

                var offset = rule.TimeZone.GetUtcOffset(executionDateTime);
                return new DateTimeOffset(executionDateTime, offset);
            }

            throw new InvalidOperationException("Unable to calculate next monthly execution within a one-year window.");
        }


        /// <summary>
        /// Attempts to create a local DateTime for the given year, month and rule day.
        /// Returns false if the day does not exist in the month.
        /// </summary>
        private static bool TryCreateLocalDateTime(int year, int month, int day, ReminderRule rule, out DateTime result)
        {
            try
            {
                result = new DateTime(
                    year,
                    month,
                    day,
                    rule.TimeOfDay.Hour,
                    rule.TimeOfDay.Minute,
                    0,
                    DateTimeKind.Unspecified
                );

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                result = default;
                return false;
            }
        }

    }
}
