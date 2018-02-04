using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Search
{
    public interface ICondition
    {
        bool Satisfies(Task task);
    }
}
