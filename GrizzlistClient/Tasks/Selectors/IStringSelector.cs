using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Selectors
{
    public interface IStringSelector
    {
        string Select(Task task);
    }
}
