using System;
using System.Collections.Generic;

namespace RapogGenerator.Shared.Comparers
{
    class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
