namespace Tempora.Core
{
    /// <summary>
    /// Represents a scheduling rule that defines when a reminder should occur.
    /// A reminder rule describes recurrence, time of day, and time zone,
    /// but does not perform any execution.
    /// </summary>
    public sealed class ReminderRule
    {
        private ReminderRule() { }

        /// <summary>
        /// Gets the days of the week on which the reminder is scheduled to occur.
        /// </summary>
        public IReadOnlyCollection<DayOfWeek>? DaysOfWeek { get; private set; }

        /// <summary>
        /// Gets the day of the month on which the reminder is scheduled to occur.
        /// </summary>
        public int? DayOfMonth { get; private set; }

        /// <summary>
        /// Gets the time of day at which the reminder should occur.
        /// </summary>
        public TimeOnly TimeOfDay { get; private set; }

        /// <summary>
        /// Gets the time zone in which the reminder rule is evaluated.
        /// </summary>
        public TimeZoneInfo TimeZone { get; private set; } = TimeZoneInfo.Utc;

        /// <summary>
        /// Creates a weekly reminder rule for the specified days of the week.
        /// </summary>
        /// <param name="days">
        /// One or more days of the week on which the reminder should occur.
        /// </param>
        /// <returns>A new <see cref="ReminderRule"/> configured for weekly recurrence.</returns>
        public static ReminderRule Weekly(params DayOfWeek[] days)
        {
            return new ReminderRule
            {
                DaysOfWeek = days
            };
        }

        /// <summary>
        /// Specifies the time of day at which the reminder should occur.
        /// </summary>
        /// <param name="hour">Hour component (0–23).</param>
        /// <param name="minute">Minute component (0–59).</param>
        /// <returns>The current <see cref="ReminderRule"/> instance.</returns>
        public ReminderRule At(int hour, int minute)
        {
            this.TimeOfDay = new TimeOnly(hour, minute);
            return this;
        }

        /// <summary>
        /// Specifies the time zone in which the reminder rule should be evaluated.
        /// </summary>
        /// <param name="timeZoneId">
        /// A valid time zone identifier, such as <c>UTC</c> or <c>Europe/Sofia</c>.
        /// </param>
        /// <returns>The current <see cref="ReminderRule"/> instance.</returns>
        public ReminderRule InTimeZone(string timeZoneId)
        {
            this.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return this;
        }
    }
}
