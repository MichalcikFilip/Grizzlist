using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Selectors
{
    class ClosedSelector : IStringSelector
    {
        public string Select(Task task)
        {
            return task.Closed.ToShortDateString();
        }
    }
}
