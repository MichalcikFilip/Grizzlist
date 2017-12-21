using Grizzlist.Tasks;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Selectors
{
    public interface IBackgroundSelector
    {
        Color Select(Task task, bool selected);
    }
}
