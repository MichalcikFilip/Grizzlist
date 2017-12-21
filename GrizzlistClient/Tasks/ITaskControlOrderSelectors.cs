using System;

namespace Grizzlist.Client.Tasks
{
    public interface ITaskControlOrderSelectors
    {
        ITaskControlOrderSelectors AddOrderSelector(Func<TaskControl, object> keySelector, bool descending);
        void ClearOrderSelectors();
    }
}
