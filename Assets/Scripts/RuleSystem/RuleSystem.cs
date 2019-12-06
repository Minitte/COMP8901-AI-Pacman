using System.Collections.Generic;

namespace RuleSystem
{
    public class RuleSystem 
    {
        public List<Rule> rules;

        public RuleSystem()
        {
            rules = new List<Rule>();
        }

        public RuleSystem(params Rule[] rules)
        {
            this.rules = new List<Rule>();
            this.rules.AddRange(rules);
        }

        public void AddRule(Rule r)
        {
            rules.Add(r);
        }

        public string Eval(Dictionary<string, int> data)
        {
            Rule best = null;

            foreach (Rule r in rules)
            {
                if (r.Eval(data))
                {
                    if (best == null || best.ConditionCount < r.ConditionCount)
                    {
                        best = r;
                    }
                }
            }

            if (best == null) return null;

            return best.result;
        }
    }

    
}
