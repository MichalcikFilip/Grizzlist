using Grizzlist.Tasks.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.FileSystem.Persistent.Tasks.Templates
{
    [Serializable]
    class PersistentConditionOperator : PersistentCondition
    {
        public ConditionOperatorType Type { get; set; }
        public List<PersistentCondition> Conditions { get; set; }

        public override Condition Convert()
        {
            ConditionOperator condition = new ConditionOperator() { ID = ID, Type = Type };

            condition.Conditions.AddRange(Conditions.Select(x => x.Convert()));

            return condition;
        }

        public override PersistentCondition Convert(Condition entity)
        {
            ConditionOperator condition = entity as ConditionOperator;

            if (condition != null)
            {
                ID = condition.ID;
                Type = condition.Type;
                Conditions = condition.Conditions.Select(x => x is ConditionOperator ? new PersistentConditionOperator().Convert((Condition)x) : new PersistentConditionValue().Convert((Condition)x)).ToList();
            }

            return this;
        }
    }
}
