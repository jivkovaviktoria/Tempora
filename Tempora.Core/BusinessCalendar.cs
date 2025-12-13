namespace Tempora.Core
{
    /// <summary>
    /// Represents a business calendar used to determine whether specific dates
    /// are valid execution days for reminders.
    /// </summary>
    public sealed class BusinessCalendar
    {
        /// <summary>
        /// Creates a default business calendar with no exclusions.
        /// </summary>
        /// <returns>A default <see cref="BusinessCalendar"/> instance.</returns>
        public static BusinessCalendar Default()
        {
            return new BusinessCalendar();
        }
    }
}
