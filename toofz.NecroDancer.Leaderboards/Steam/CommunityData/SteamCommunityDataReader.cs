using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace toofz.NecroDancer.Leaderboards.Steam.CommunityData
{
    internal static class SteamCommunityDataReader
    {
        public static IEnumerable<Entry> ReadLeaderboardEntries(Stream stream)
        {
            var doc = XDocument.Load(stream);
            var responseEl = doc.Element("response") ?? throw new XmlException("Unable to find element 'response'.");
            var entriesEl = responseEl.Element("entries") ?? throw new XmlException("Unable to find element 'entries'.");

            var entries = new List<Entry>();

            foreach (var entryEl in entriesEl.Elements("entry"))
            {
                var entry = new Entry();

                try
                {
                    entry.SteamId = (long)entryEl.Element("steamid");
                    entry.Score = (int)entryEl.Element("score");
                    entry.Rank = (int)entryEl.Element("rank");
                    entry.ReplayId = ((ulong)entryEl.Element("ugcid")).ToReplayId();

                    var details = (from d in (string)entryEl.Element("details")
                                   select int.Parse(d.ToString(), NumberStyles.HexNumber))
                                   .ToList();

                    entry.Zone = details[1];
                    entry.Level = details[9];
                }
                catch (Exception ex)
                {
                    throw new XmlException("Error while parsing element 'entry'.", ex);
                }

                entries.Add(entry);
            }

            return entries;
        }
    }
}
