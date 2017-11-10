namespace toofz.NecroDancer.Leaderboards.Steam
{
    public static class UInt64Extensions
    {
        public static long? ToReplayId(this ulong ugcId)
        {
            var replayId = (long)ugcId;
            switch (replayId)
            {
                case -1:
                case 0: return null;
                default: return replayId;
            }
        }
    }
}
