using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Except implicitly runs a distinct on the result set (same as in SQL).
        /// This method is the equivalent of EXCEPT ALL in SQL.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExceptAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return ExceptAll(first, second, null);
        }

        /// <summary>
        /// Except implicitly runs a distinct on the result set (same as in SQL).
        /// This method is the equivalent of EXCEPT ALL in SQL.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExceptAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) { throw new ArgumentNullException("first"); }
            if (second == null) { throw new ArgumentNullException("second"); }

            return ExceptAllImplementation(first, second, comparer);
        }

        private static IEnumerable<TSource> ExceptAllImplementation<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {

            var valueCounter = new ValueCounter<TSource>(second, comparer);

            // Do not convert to Where, this enumerates wrong the second time
            foreach (TSource s in first)
            {
                if (!valueCounter.Remove(s))
                {
                    yield return s;
                }
            }
        }

        public static IEnumerable<TSource> IntersectAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return IntersectAll(first, second, null);
        }

        public static IEnumerable<TSource> IntersectAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) { throw new ArgumentNullException("first"); }
            if (second == null) { throw new ArgumentNullException("second"); }

            return IntersectAllImplementation(first, second, comparer);
        }

        private static IEnumerable<TSource> IntersectAllImplementation<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {

            var valueCounter = new ValueCounter<TSource>(second, comparer);

            // Do not convert to Where, this enumerates wrong the second time
            foreach (TSource s in first)
            {
                if (valueCounter.Remove(s))
                {
                    yield return s;
                }
            }
        }

        public static IEnumerable<TSource> UnionAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return UnionAll(first, second, null);
        }

        public static IEnumerable<TSource> UnionAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) { throw new ArgumentNullException("first"); }
            if (second == null) { throw new ArgumentNullException("second"); }

            var firstCache = first.ToList();
            return firstCache.Concat(second.ExceptAll(firstCache, comparer));
        }
    }

    public class ValueCounter<T> : IEnumerable<KeyValuePair<T, int>>
    {
        private readonly Dictionary<T, int> _valueCounter;
        private int _nullCount;

        public ValueCounter(IEnumerable<T> values, IEqualityComparer<T> comparer)
        {
            _valueCounter = new Dictionary<T, int>(comparer != null ? comparer : EqualityComparer<T>.Default);
            if (values == null)
                return;
            foreach (var value in values)
            {
                Add(value);
            }
        }

        public ValueCounter(IEqualityComparer<T> comparer) : this(null, comparer)
        {
        }

        public ValueCounter(IEnumerable<T> values) : this(values, null)
        {
        }

        public ValueCounter() : this(null, null)
        {
        }

        public void Add(T value)
        {
            if (value == null)
            {
                _nullCount++;
            }
            else
            {
                int count;
                if (_valueCounter.TryGetValue(value, out count))
                {
                    // Double lookup is faster then creating a StrongBox
                    _valueCounter[value] = count + 1;
                }
                else
                {
                    _valueCounter.Add(value, 1);
                }
            }
        }

        public bool Remove(T value)
        {
            if (value == null)
            {
                if (_nullCount > 0)
                {
                    _nullCount--;
                    return true;
                }
            }
            else
            {
                int count;
                if (_valueCounter.TryGetValue(value, out count))
                {
                    if (count == 0)
                    {
                        return false;
                    }
                    // Double lookup is faster then creating a StrongBox
                    _valueCounter[value] = count - 1;
                    return true;
                }
            }
            return false;
        }

        public int GetCount(T value)
        {
            if (value == null)
            {
                return _nullCount;
            }
            int result;
            _valueCounter.TryGetValue(value, out result);
            return result;
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            return _valueCounter.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
