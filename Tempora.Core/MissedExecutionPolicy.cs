namespace Tempora.Core
{
    /// <summary>
    /// Defines how missed reminder executions should be handled when calculating
    /// the next valid execution time.
    /// </summary>
    public enum MissedExecutionPolicy
    {
        /// <summary>
        /// Calculates only the most recent missed execution.
        /// </summary>
        RunLastOnly,

        /// <summary>
        /// Calculates all missed executions where applicable.
        /// </summary>
        RunAll,

        /// <summary>
        /// Skips all missed executions and returns the next future execution.
        /// </summary>
        Skip
    }
}
