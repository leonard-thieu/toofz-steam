using System;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Character
    {
        [ExcludeFromCodeCoverage]
        Character() { }

        public Character(int characterId, string name, string displayName)
        {
            CharacterId = characterId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
    }
}
