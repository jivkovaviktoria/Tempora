namespace Tempora.Core.Exceptions
{
    /// <summary>
    /// Thrown when a reminder rule is invalid or improperly configured.
    /// </summary>
    public sealed class InvalidReminderRuleException : TemporaException
    {
        public InvalidReminderRuleException(string message)
            : base(message)
        { }
    }
}
