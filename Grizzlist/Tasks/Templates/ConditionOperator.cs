using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Tasks.Templates
{
    public class ConditionOperator : Condition
    {
        private List<ICondition> conditions = new List<ICondition>();

        public ConditionOperatorType Type { get; set; }
        public List<ICondition> Conditions { get { return conditions; } }

        public override bool Satisfies(DateTime date)
        {
            switch(Type)
            {
                case ConditionOperatorType.And:
                    return conditions.All(x => x.Satisfies(date));
                case ConditionOperatorType.Or:
                    return conditions.Any(x => x.Satisfies(date));
                default:
                    return false;
            }
        }

        public override bool Created(DateTime date, DateTime last)
        {
            switch (Type)
            {
                case ConditionOperatorType.And:
                    return conditions.All(x => x.Created(date, last));
                case ConditionOperatorType.Or:
                    return conditions.Any(x => x.Created(date, last));
                default:
                    return false;
            }
        }
    }
}
