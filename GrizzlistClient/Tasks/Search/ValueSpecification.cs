using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.Search
{
    class ValueSpecification<T> : IValueSpecification where T : struct
    {
        public ValueType Type { get; private set; }
        public Func<Task, T> ValueSelector { get; private set; }

        public ValueSpecification(ValueType type, Func<Task, T> valueSelector)
        {
            Type = type;
            ValueSelector = valueSelector;
        }

        public ICondition CreateCondition(object value)
        {
            return new ConditionValue<T>() { Value = (T)value, Specification = this };
        }
    }
}
