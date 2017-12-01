using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer run type.
    /// </summary>
    public sealed class Run
    {
        // Required for Entity Framework
        private Run() { }

        /// <summary>
        /// Initializes an instance of the <see cref="Run"/> class.
        /// </summary>
        /// <param name="runId">>A unique identifier for the run.</param>
        /// <param name="name">The run's short name.</param>
        /// <param name="displayName">The run's display name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="displayName"/> is null.
        /// </exception>
        public Run(int runId, string name, string displayName) : this()
        {
            RunId = runId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// A unique identifier for the run.
        /// </summary>
        public int RunId { get; private set; }
        /// <summary>
        /// The run's short name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The run's display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
