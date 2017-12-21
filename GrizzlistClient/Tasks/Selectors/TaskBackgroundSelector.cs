using Grizzlist.Tasks;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Selectors
{
    public class TaskBackgroundSelector : IBackgroundSelector
    {
        public virtual Color Select(Task task, bool selected)
        {
            if (selected)
                return Color.FromRgb(216, 237, 255);
            else
                return Colors.White;
        }
    }
}
