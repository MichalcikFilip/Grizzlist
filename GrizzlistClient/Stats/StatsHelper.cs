using Grizzlist.Client.Persistent;
using Grizzlist.Persistent;
using Grizzlist.Stats;
using Grizzlist.Tasks;
using System.Linq;

namespace Grizzlist.Client.Stats
{
    static class StatsHelper
    {
        public static void Update(UpdateStatsData update)
        {
            using (IRepository<StatsManager, long> repository = PersistentFactory.GetContext().GetRepository<StatsManager, long>())
            {
                StatsManager manager = repository.GetAll().FirstOrDefault();

                if (manager == null)
                {
                    manager = new StatsManager();
                    repository.Add(manager);
                }

                manager.Update(update);
                repository.Update(manager);
            }
        }

        public static UpdateStatsData PriorityUpdate(TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.VeryHigh:
                    return StatsData.TaskVeryHighPriority;
                case TaskPriority.High:
                    return StatsData.TaskHighPriority;
                case TaskPriority.Normal:
                    return StatsData.TaskNormalPriority;
                case TaskPriority.Low:
                    return StatsData.TaskLowPriority;
                case TaskPriority.VeryLow:
                    return StatsData.TaskVeryLowPriority;
                default:
                    return null;
            }
        }
    }
}
