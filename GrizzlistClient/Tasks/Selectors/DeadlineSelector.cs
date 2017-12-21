using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.Selectors
{
    class DeadlineSelector : IStringSelector
    {
        public string Select(Task task)
        {
            return $"{task.Deadline.ToShortDateString()} ({DaysLeft(DateTime.Today, task.Deadline)})";
        }

        private string DaysLeft(DateTime now, DateTime deadline)
        {
            if (now > deadline)
                return "Exceeded";
            else if (now == deadline)
                return "Today";

            int days = (int)(deadline - now).TotalDays;
            return days + (days == 1 ? " day" : " days") + " left";
        }
    }
}
