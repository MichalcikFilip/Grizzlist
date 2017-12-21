using Grizzlist.Persistent;
using Grizzlist.Stats;
using System;

namespace Grizzlist.FileSystem.Persistent.Stats
{
    [Serializable]
    struct PersistentStatsData : IPersistentEntity<long>, IConvertible<StatsData, PersistentStatsData, long>, IIncrementalKey<long>
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

        public StatsData Convert()
        {
            return new StatsData() { ID = ID, Date = Date, TasksCreated = TasksCreated, TasksCreatedFromTemplate = TasksCreatedFromTemplate, TasksClosed = TasksClosed, TasksArchived = TasksArchived, TasksRemoved = TasksRemoved, TasksVeryLowPriority = TasksVeryLowPriority, TasksLowPriority = TasksLowPriority, TasksNormalPriority = TasksNormalPriority, TasksHighPriority = TasksHighPriority, TasksVeryHighPriority = TasksVeryHighPriority };
        }

        public PersistentStatsData Convert(StatsData entity)
        {
            ID = entity.ID;
            Date = entity.Date;

            TasksCreated = entity.TasksCreated;
            TasksCreatedFromTemplate = entity.TasksCreatedFromTemplate;
            TasksClosed = entity.TasksClosed;
            TasksArchived = entity.TasksArchived;
            TasksRemoved = entity.TasksRemoved;
            TasksVeryLowPriority = entity.TasksVeryLowPriority;
            TasksLowPriority = entity.TasksLowPriority;
            TasksNormalPriority = entity.TasksNormalPriority;
            TasksHighPriority = entity.TasksHighPriority;
            TasksVeryHighPriority = entity.TasksVeryHighPriority;

            return this;
        }

        public bool CanAutoIncrement()
        {
            return true;
        }

        public long GetNewKey(long lastKey)
        {
            return ++lastKey;
        }
    }
}
