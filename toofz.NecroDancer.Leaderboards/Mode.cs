using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer game mode.
    /// </summary>
    public sealed class Mode
    {
        // Required for Entity Framework
        private Mode() { }

        /// <summary>
        /// Initializes an instance of the <see cref="Mode"/> class.
        /// </summary>
        /// <param name="modeId">A unique identifier for the game mode.</param>
        /// <param name="name">The game mode's short name.</param>
        /// <param name="displayName">The game mode's display name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="displayName"/> is null.
        /// </exception>
        public Mode(int modeId, string name, string displayName) : this()
        {
            ModeId = modeId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// A unique identifier for the game mode.
        /// </summary>
        public int ModeId { get; private set; }
        /// <summary>
        /// The game mode's short name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The game mode's display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
