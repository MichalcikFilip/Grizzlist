using Grizzlist.Tasks.Templates;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Templates
{
    [Serializable]
    class PersistentConditionValue : PersistentCondition
    {
        public ConditionValueType Type { get; set; }
        public short Value { get; set; }

        public override Condition Convert()
        {
            return new ConditionValue() { ID = ID, Type = Type, Value = Value };
        }

        public override PersistentCondition Convert(Condition entity)
        {
            ConditionValue condition = entity as ConditionValue;

            if (condition != null)
            {
                ID = condition.ID;
                Type = condition.Type;
                Value = condition.Value;
            }

            return this;
        }
    }
}
