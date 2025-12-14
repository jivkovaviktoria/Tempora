namespace Tempora.Core
{
    /// <summary>
    /// Represents a business calendar used to determine whether specific dates
    /// are valid execution days for reminders.
    /// </summary>
    public sealed class BusinessCalendar
    {
        private readonly bool _excludeWeekends;

        private BusinessCalendar(bool excludeWeekends)
        {
            _excludeWeekends = excludeWeekends;
        }

        /// <summary>
        /// Creates a default business calendar with no exclusions.
        /// </summary>
        /// <returns>A default <see cref="BusinessCalendar"/> instance.</returns>
        public static BusinessCalendar Default()
        {
            return new BusinessCalendar(excludeWeekends: false);
        }

        /// <summary>
        /// Returns a new <see cref="BusinessCalendar"/> instance that excludes
        /// Saturdays and Sundays from valid execution days.
        /// </summary>
        /// <returns>
        /// A new <see cref="BusinessCalendar"/> with weekend exclusion enabled.
        /// </returns>
        public BusinessCalendar ExcludeWeekends()
        {
            return new BusinessCalendar(excludeWeekends: true);
        }
        /// <summary>
        /// Determines whether the specified date is considered a business day.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the date is a business day; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBusinessDay(DateOnly date)
        {
            if (_excludeWeekends)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adjusts the specified date to a valid business day according to
        /// the calendar's configuration.
        /// </summary>
        /// <param name="date">The date to adjust.</param>
        /// <returns>A valid business day.</returns>
        public DateOnly AdjustToBusinessDay(DateOnly date)
        {
            var adjustedDate = date;

            while (!IsBusinessDay(adjustedDate))
            {
                adjustedDate = adjustedDate.AddDays(1);
            }

            return adjustedDate;
        }

    }
}
