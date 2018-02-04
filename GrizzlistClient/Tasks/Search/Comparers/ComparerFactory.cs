namespace Grizzlist.Client.Tasks.Search.Comparers
{
    static class ComparerFactory
    {
        public static IComparer CreateComparer(OperatorType type)
        {
            switch (type)
            {
                case OperatorType.Equals:
                    return new Equals();
                case OperatorType.NotEquals:
                    return new NotEquals();
                case OperatorType.GreaterThan:
                    return new GreaterThan();
                case OperatorType.LessThan:
                    return new LessThan();
                case OperatorType.Contains:
                    return new Contains();
                case OperatorType.NotContains:
                    return new NotContains();
                default:
                    return null;
            }
        }
    }
}
