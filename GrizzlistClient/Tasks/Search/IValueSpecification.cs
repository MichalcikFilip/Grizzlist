namespace Grizzlist.Client.Tasks.Search
{
    interface IValueSpecification
    {
        ICondition CreateCondition(object value, OperatorType operatorType);
    }
}
