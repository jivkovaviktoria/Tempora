namespace Tempora.Core
{
    /// <summary>
    /// Provides information about holidays for a specific calendar or region.
    /// </summary>
    public interface IHolidayProvider
    {
        /// <summary>
        /// Determines whether the specified date is a holiday.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the date is a holiday; otherwise, <c>false</c>.
        /// </returns>
        bool IsHoliday(DateOnly date);
    }
}
