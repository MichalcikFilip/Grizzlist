using Grizzlist.Persistent;
using System;

namespace Grizzlist.Stats
{
    public delegate StatsData UpdateStatsData(StatsData data);

    public struct StatsData : IPersistentEntity<long>
    {
        public long ID { get; set; }
        public DateTime Date { get; set; }

        public long TasksCreated { get; set; }
        public long TasksCreatedFromTemplate { get; set; }
        public long TasksClosed { get; set; }
        public long TasksArchived { get; set; }
        public long TasksRemoved { get; set; }
        public long TasksVeryLowPriority { get; set; }
        public long TasksLowPriority { get; set; }
        public long TasksNormalPriority { get; set; }
        public long TasksHighPriority { get; set; }
        public long TasksVeryHighPriority { get; set; }

        public static StatsData operator +(StatsData d1, StatsData d2)
        {
            return new StatsData() { TasksCreated = d1.TasksCreated + d2.TasksCreated, TasksCreatedFromTemplate = d1.TasksCreatedFromTemplate + d2.TasksCreatedFromTemplate, TasksClosed = d1.TasksClosed + d2.TasksClosed, TasksArchived = d1.TasksArchived + d2.TasksArchived, TasksRemoved = d1.TasksRemoved + d2.TasksRemoved, TasksVeryLowPriority = d1.TasksVeryLowPriority + d2.TasksVeryLowPriority, TasksLowPriority = d1.TasksLowPriority + d2.TasksLowPriority, TasksNormalPriority = d1.TasksNormalPriority + d2.TasksNormalPriority, TasksHighPriority = d1.TasksHighPriority + d2.TasksHighPriority, TasksVeryHighPriority = d1.TasksVeryHighPriority + d2.TasksVeryHighPriority };
        }

        public StatsData(DateTime date)
        {
            ID = 0;
            Date = date;

            TasksCreated = 0;
            TasksCreatedFromTemplate = 0;
            TasksClosed = 0;
            TasksArchived = 0;
            TasksRemoved = 0;
            TasksVeryLowPriority = 0;
            TasksLowPriority = 0;
            TasksNormalPriority = 0;
            TasksHighPriority = 0;
            TasksVeryHighPriority = 0;
        }

        public static UpdateStatsData TaskCreated { get { return d => { d.TasksCreated++; return d; }; } }
        public static UpdateStatsData TaskCreatedFromTemplate { get { return d => { d.TasksCreatedFromTemplate++; return d; }; } }
        public static UpdateStatsData TaskClosed { get { return d => { d.TasksClosed++; return d; }; } }
        public static UpdateStatsData TaskArchived { get { return d => { d.TasksArchived++; return d; }; } }
        public static UpdateStatsData TaskRemoved { get { return d => { d.TasksRemoved++; return d; }; } }
        public static UpdateStatsData TaskVeryLowPriority { get { return d => { d.TasksVeryLowPriority++; return d; }; } }
        public static UpdateStatsData TaskLowPriority { get { return d => { d.TasksLowPriority++; return d; }; } }
        public static UpdateStatsData TaskNormalPriority { get { return d => { d.TasksNormalPriority++; return d; }; } }
        public static UpdateStatsData TaskHighPriority { get { return d => { d.TasksHighPriority++; return d; }; } }
        public static UpdateStatsData TaskVeryHighPriority { get { return d => { d.TasksVeryHighPriority++; return d; }; } }
    }
}
