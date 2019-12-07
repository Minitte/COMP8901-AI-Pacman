using System.IO;
using System.Collections.Generic;

namespace RuleSystem
{
    public static class RuleSetReader
    {
        public const string COMMENT_PREFIX = "//";
        public const char RESULT_SPLITER = '?';
        public const char COND_SPLITER = '&';

        public static Rule[] ReadRuleFile(string path)
        {
            List<Rule> rules = new List<Rule>();

            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.StartsWith(COMMENT_PREFIX)) continue;

                rules.Add(PhaseRule(line.Trim()));
            }

            return rules.ToArray();
        }

        public static Rule[] ReadRuleString(string rulesStr)
        {
            List<Rule> rules = new List<Rule>();

            StringReader sr = new StringReader(rulesStr);

            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine();

                if (line.StartsWith(COMMENT_PREFIX)) continue;

                rules.Add(PhaseRule(line.Trim()));
            }

            return rules.ToArray();
        }

        public static Rule PhaseRule(string s)
        {
            string[] cond_resu = s.Split(RESULT_SPLITER);
            string[] conds_str = cond_resu[0].Split(COND_SPLITER);
            string result = cond_resu[1].Trim();

            Condition[] conditions = new Condition[conds_str.Length];

            int i = 0;
            foreach (string cond in conds_str)
            {
                string[] args = cond.Trim().Split(' ');

                conditions[i++] = new Condition(args[0].ToLower().Trim(), ToConditionType(args[1].Trim()), int.Parse(args[2]));
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
