using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Search
{
    class ConditionValue : ICondition
    {
        public bool Satisfies(Task task)
        {
            return false;
        }
    }
}
