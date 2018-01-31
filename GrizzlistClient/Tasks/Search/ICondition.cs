using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Search
{
    interface ICondition
    {
        bool Satisfies(Task task);
    }
}
