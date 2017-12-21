using Grizzlist.Stats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.FileSystem.Persistent.Stats
{
    [Serializable]
    class PersistentStatsManager : LongIncrementalKey, IConvertible<StatsManager, PersistentStatsManager, long>
    {
        public Dictionary<DateTime, PersistentStatsData> Data { get; set; }

        public StatsManager Convert()
        {
            return new StatsManager(Data.ToDictionary(x => x.Key, x => x.Value.Convert())) { ID = ID };
        }

        public PersistentStatsManager Convert(StatsManager entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Data = entity.GetData().ToDictionary(x => x.Date, x => new PersistentStatsData().Convert(x));
            }

            return this;
        }
    }
}
