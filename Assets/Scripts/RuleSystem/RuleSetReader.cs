using System.IO;
using System.Collections.Generic;

namespace RuleSystem
{
    public static class RuleSetReader
    {
        public const char RESULT_SPLITER = '?';
        public const char COND_SPLITER = '&';

        public static Rule[] ReadRuleFile(string path)
        {
            List<Rule> rules = new List<Rule>();

            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                rules.Add(PhaseRule(line));
            }

            return rules.ToArray();
        }

        public static Rule PhaseRule(string s)
        {
            string[] cond_resu = s.Split(RESULT_SPLITER);
            string[] conds_str = cond_resu[0].Split(COND_SPLITER);
            string result = cond_resu[1];

            Condition[] conditions = new Condition[conds_str.Length];

            int i = 0;
            foreach (string cond in conds_str)
            {
                string[] args = cond.Split(' ');

                conditions[i++] = new Condition(args[0].ToLower(), ToConditionType(args[1]), int.Parse(args[2]));
            }

            return new Rule(result, conditions);
        }

        private static ConditionType ToConditionType(string s)
        {
            if (s == "=") return ConditionType.EQUALS;
            else if (s == ">") return ConditionType.GREATER;
            else return ConditionType.LESS;
        }
    }
}
