using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer character.
    /// </summary>
    public sealed class Character
    {
        // Required for Entity Framework
        private Character() { }

        /// <summary>
        /// Initializes an instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="characterId">A unique identifier for the character.</param>
        /// <param name="name">The character's short name.</param>
        /// <param name="displayName">The character's display name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="displayName"/> is null.
        /// </exception>
        public Character(int characterId, string name, string displayName) : this()
        {
            CharacterId = characterId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// A unique identifier for the character.
        /// </summary>
        public int CharacterId { get; private set; }
        /// <summary>
        /// The character's short name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The character's display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
