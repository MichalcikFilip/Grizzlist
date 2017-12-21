using Grizzlist.Tasks;
using System.Collections.Generic;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks
{
    static class TaskPriorityColors
    {
        private static Dictionary<TaskPriority, Color> colors = new Dictionary<TaskPriority, Color>();

        static TaskPriorityColors()
        {
            colors.Add(TaskPriority.VeryHigh, Color.FromArgb(255, 255, 0, 0));
            colors.Add(TaskPriority.High, Color.FromArgb(120, 255, 0, 0));
            colors.Add(TaskPriority.Normal, Color.FromArgb(120, 0, 148, 255));
            colors.Add(TaskPriority.Low, Color.FromArgb(120, 76, 255, 0));
            colors.Add(TaskPriority.VeryLow, Color.FromArgb(255, 76, 255, 0));
        }

        public static Color GetColor(TaskPriority priority)
        {
            if (colors.ContainsKey(priority))
                return colors[priority];

            return Colors.White;
        }
    }
}
