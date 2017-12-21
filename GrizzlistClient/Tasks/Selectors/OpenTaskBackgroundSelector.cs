using Grizzlist.Tasks;
using System;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Selectors
{
    public class OpenTaskBackgroundSelector : TaskBackgroundSelector
    {
        public override Color Select(Task task, bool selected)
        {
            DateTime today = DateTime.Today;

            if (task.Deadline < today)
            {
                if (selected)
                    return Color.FromRgb(255, 127, 127);
                else
                    return Color.FromRgb(255, 191, 191);
            }
            else if (task.Deadline == today)
            {
                if (selected)
                    return Color.FromRgb(255, 233, 127);
                else
                    return Color.FromRgb(255, 244, 191);
            }

            return base.Select(task, selected);
        }
    }
}
