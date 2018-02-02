using Grizzlist.Client.Validators;
using Grizzlist.Tasks;
using System;
using System.Windows;

namespace Grizzlist.Client.Tasks.Search
{
    class ValueSpecification<T> : IValueSpecification
    {
        public ValueType Type { get; private set; }
        public Func<Task, T> ValueSelector { get; private set; }
        public UIElement Control { get; private set; }
        public Func<UIElement, T> ControlValueSelector { get; private set; }
        public OperatorType[] Operators { get; private set; }
        public IValidator[] Validators { get; private set; }

        public ValueSpecification(ValueType type, Func<Task, T> valueSelector, UIElement control, Func<UIElement, T> controlValueSelector, OperatorType[] operators, IValidator[] validators)
        {
            Type = type;
            ValueSelector = valueSelector;
            Control = control;
            ControlValueSelector = controlValueSelector;
            Operators = operators;
            Validators = validators;
        }

        public ICondition CreateCondition(OperatorType operatorType)
        {
            return new ConditionValue<T>() { Value = ControlValueSelector.Invoke(Control), OperatorType = operatorType, ValueSelector = ValueSelector };
        }
    }
}
