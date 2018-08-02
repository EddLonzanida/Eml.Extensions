using System;
using System.Collections.Generic;

namespace Eml.Extensions
{
    public enum eSortFlag
    {
        LessThan = -1,
        Equal = 0,
        GreaterThan = 1
    }

    public class LambdaComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _lambdaComparer;

        public LambdaComparer(Func<T, T, int> lambdaComparer)
        {
            _lambdaComparer = lambdaComparer.CheckNotNull(nameof(lambdaComparer));
        }

        public int Compare(T x, T y)
        {
            return _lambdaComparer(x, y);
        }
    }
}
