using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Search
{
    class ConditionValue<T> : ICondition where T : struct
    {
        public T Value { get; set; }
        public ValueSpecification<T> Specification { get; set; }

        public bool Satisfies(Task task)
        {
            return false;
        }
    }
}
