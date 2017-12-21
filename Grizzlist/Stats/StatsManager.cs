using Grizzlist.Extensions;
using Grizzlist.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Stats
{
    public class StatsManager : PersistentEntity<long>
    {
        private Dictionary<DateTime, StatsData> data = new Dictionary<DateTime, StatsData>();

        public StatsManager(Dictionary<DateTime, StatsData> data = null)
        {
            if (data != null) this.data = data;
        }

        public IEnumerable<StatsData> GetData()
        {
            return data.Values;
        }

        public IEnumerable<StatsData> GetData(DateTime from, DateTime to)
        {
            return data.Values.Where(x => x.Date >= from.GetDate() && x.Date <= to.GetDate());
        }

        public void Update(UpdateStatsData update)
        {
            DateTime today = DateTime.Today;

            if (!data.ContainsKey(today))
                data.Add(today, new StatsData(today));

            data[today] = update?.Invoke(data[today]) ?? data[today];
        }
    }
}
