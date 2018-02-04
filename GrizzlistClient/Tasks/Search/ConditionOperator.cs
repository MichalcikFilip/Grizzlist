using Grizzlist.Tasks;
using Grizzlist.Tasks.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Client.Tasks.Search
{
    class ConditionOperator : ICondition
    {
        private List<ICondition> conditions = new List<ICondition>();

        public ConditionOperatorType Type { get; set; }
        public List<ICondition> Conditions { get { return conditions; } }

        public bool Satisfies(Task task)
        {
            switch (Type)
            {
                case ConditionOperatorType.And:
                    return conditions.All(x => x.Satisfies(task));
                case ConditionOperatorType.Or:
                    return conditions.Any(x => x.Satisfies(task));
                default:
                    return false;
            }
        }
    }
}
