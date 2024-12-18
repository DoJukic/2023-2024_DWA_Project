using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooManyUtils
{
    public class ReadonlySortedSet<T> : IEnumerable<T>
    {
        private SortedSet<T> internalSet;

        public IComparer<T> Comparer { get { return internalSet.Comparer; } }
        public int Count { get { return internalSet.Count; } }
        public T? Max { get { return internalSet.Max; } }
        public T? Min { get { return internalSet.Min; } }

        public ReadonlySortedSet(SortedSet<T> internalSet)
        {
            this.internalSet = internalSet;
        }

        public bool Contains(T item)
        {
            return internalSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return internalSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalSet.GetEnumerator();
        }

        public bool TryGetValue(T equalValue, out T? actualValue)
        {
            return internalSet.TryGetValue(equalValue, out actualValue);
        }
    }
}
