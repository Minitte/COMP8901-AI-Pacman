using System.Collections.Generic;

namespace NGram
{
    public class NGramString
    {
        private const char COMBO_SPILTER = '/';
        private Dictionary<string, NGramSet> m_combos;

        private int m_total;

        public NGramString()
        {
            m_combos = new Dictionary<string, NGramSet>();
        }

        public void Add(string before, string after)
        {
            before = before.ToLower();
            after = after.ToLower();

            if (m_combos.ContainsKey(before))
            {
                m_combos[before].Add(after);
            }
            else
            {
                m_combos.Add(before, new NGramSet(before));
                m_combos[before].Add(after);
            }

            m_total++;
        }

        public string Predict(string before)
        {
            before = before.ToLower();

            string best_before = "";
            int best_matchLength = 0;

            foreach (string s in m_combos.Keys)
            {
                // does not apply so skip.
                if (!before.StartsWith(s)) continue;

                // current best is more specific
                if (best_matchLength > s.Length) continue;

                best_before = m_combos[s].GetHighestProp();
                best_matchLength = s.Length;
            }

            return best_before;
        }

        class NGramSet
        {
            public string before;

            private List<string> afterList;

            private List<int> countList;

            public NGramSet(string before)
            {
                this.before = before;

                afterList = new List<string>();
                countList = new List<int>();
            }

            public void Add(string after)
            {
                if (afterList.Contains(after))
                {
                    int index = afterList.IndexOf(after);
                    countList[index] = countList[index] + 1;
                }
                else
                {
                    afterList.Add(after);
                    countList.Add(1);
                }
            }

            public string GetHighestProp()
            {
                string best_after = afterList[0];
                float best_prop = (float)countList[0] / (float)countList.Count;

                for (int i = 0; i < afterList.Count; i++)
                {
                    float prop = (float)countList[i] / (float)countList.Count;

                    if (prop > best_prop)
                    {
                        best_prop = prop;
                        best_after = afterList[i];
                    }
                }

                return best_after;
            }
        }
    }
}