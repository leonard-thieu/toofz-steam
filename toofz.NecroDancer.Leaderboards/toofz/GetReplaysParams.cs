﻿namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class GetReplaysParams : IPagination
    {
        public int? Version { get; set; }
        public int? ErrorCode { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}