using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Character
    {
        private Character() { }

        public Character(int characterId, string name, string displayName) : this()
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
