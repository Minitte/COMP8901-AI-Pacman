using System.Collections;
using System.Collections.Generic;

namespace DP.Collections
{

    public class PriorityQueue<T>
    {
        public int Count { get { return m_list.Count; } }

        public bool Empty { get { return m_list.Count == 0; } }

        private List<T> m_list;

        private List<int> m_priority;

        public PriorityQueue()
        {
            m_list = new List<T>();
            m_priority = new List<int>();
        }

        /// <summary>
        /// Inserts the item
        /// </summary>
        /// <param name="toAdd"></param>
        /// <param name="p"></param>
        public void Insert(T toAdd, int p)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                if (m_priority[i] < p)
                {
                    m_list.Insert(i, toAdd);
                    m_priority.Insert(i, p);

                    return;
                }
            }

            m_list.Add(toAdd);
            m_priority.Add(p);
        }

        /// <summary>
        /// Returns the front of the queue
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return m_list[0];
        }

        /// <summary>
        /// Returns the front of the queue and removes it
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T temp = Peek();

            m_list.RemoveAt(0);
            m_priority.RemoveAt(0);

            return temp;
        }

        public void Clear()
        {
            m_list.Clear();
            m_priority.Clear();
        }
    }
}