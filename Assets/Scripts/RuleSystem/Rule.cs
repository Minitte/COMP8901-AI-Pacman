
using System.Collections.Generic;

namespace RuleSystem
{
    public class Rule
    {
        public string result;

        public Condition[] conditions;

        public Rule(string result, Condition[] conditions)
        {
            this.result = result.ToLower();
            this.conditions = conditions;
        }

        public int ConditionCount { get { return conditions.Length; } }

        public bool Eval(Dictionary<string, int> data)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].Eval(data)) return false;
            }

            return true;
        }
    }
}
