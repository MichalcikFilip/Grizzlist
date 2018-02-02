using Grizzlist.Client.Validators;
using System.Windows;

namespace Grizzlist.Client.Tasks.Search
{
    interface IValueSpecification
    {
        UIElement Control { get; }
        OperatorType[] Operators { get; }
        IValidator[] Validators { get; }

        ICondition CreateCondition(OperatorType operatorType);
    }
}
