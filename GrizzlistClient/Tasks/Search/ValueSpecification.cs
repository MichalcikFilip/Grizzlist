using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.Search
{
    class ValueSpecification<T> : IValueSpecification
    {
        public ValueType Type { get; private set; }
        public Func<Task, T> ValueSelector { get; private set; }

        public ValueSpecification(ValueType type, Func<Task, T> valueSelector)
        {
            Type = type;
            ValueSelector = valueSelector;
        }

        public ICondition CreateCondition(object value, OperatorType operatorType)
        {
            return new ConditionValue<T>() { Value = (T)value, OperatorType = operatorType, Specification = this };
        }
    }
}
