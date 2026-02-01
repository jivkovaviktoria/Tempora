namespace Tempora.Core.Exceptions
{
    /// <summary>
    /// Base exception type for all Tempora-specific errors.
    /// </summary>
    public abstract class TemporaException : Exception
    {
        protected TemporaException(string message)
            : base(message)
        { }
    }
}
