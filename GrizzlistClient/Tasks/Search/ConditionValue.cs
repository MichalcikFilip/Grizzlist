using Grizzlist.Client.Tasks.Search.Comparers;
using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.Search
{
    class ConditionValue<T> : ICondition
    {
        public T Value { get; set; }
        public OperatorType OperatorType { get; set; }
        public Func<Task, T> ValueSelector { get; set; }

        public bool Satisfies(Task task)
        {
            return ComparerFactory.CreateComparer(OperatorType).Compare(Value, ValueSelector(task));
        }
    }
}
