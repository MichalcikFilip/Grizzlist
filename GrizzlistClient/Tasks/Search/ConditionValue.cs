using Grizzlist.Client.Tasks.Search.Comparers;
using Grizzlist.Tasks;

namespace Grizzlist.Client.Tasks.Search
{
    class ConditionValue<T> : ICondition
    {
        public T Value { get; set; }
        public OperatorType OperatorType { get; set; }
        public ValueSpecification<T> Specification { get; set; }

        public bool Satisfies(Task task)
        {
            return ComparerFactory.CreateComparer(OperatorType).Compare(Value, Specification.ValueSelector(task));
        }
    }
}
