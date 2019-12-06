using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleSystem
{
    public class Condition
    {
        public string conditionName;

        public ConditionType condType;

        public int expected;

        public Condition(string conditionName, ConditionType condType, int expected)
        {
            this.conditionName = conditionName;
            this.condType = condType;
            this.expected = expected;
        }

        public bool Eval(Dictionary<string, int> data)
        {
            if (!data.ContainsKey(conditionName)) return false;

            switch (condType)
            {
                case ConditionType.EQUALS:
                    return data[conditionName] == expected;

                case ConditionType.GREATER:
                    return data[conditionName] > expected;

                case ConditionType.LESS:
                    return data[conditionName] < expected;
            }

            return false;
        }
    }

    public enum ConditionType
    {
        EQUALS,
        GREATER,
        LESS
    }
}
