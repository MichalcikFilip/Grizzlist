using System.Windows.Controls;

namespace Grizzlist.Client.Tasks.Selectors
{
    public interface IGroupIconsSelector
    {
        void Select(TasksGroupControl control, StackPanel panel);
    }
}
