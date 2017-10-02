using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Mode
    {
        Mode() { }

        public Mode(int modeId, string name, string displayName)
        {
            ModeId = modeId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        public int ModeId { get; private set; }
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
    }
}
