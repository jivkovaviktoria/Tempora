namespace Tempora.Core.Exceptions
{
    public sealed class SchedulingCalculationException : TemporaException
    {
        /// <summary>
        /// Thrown when a valid next execution time cannot be calculated.
        /// </summary>
        public SchedulingCalculationException(string message)
            : base(message)
        { }
    }
}
