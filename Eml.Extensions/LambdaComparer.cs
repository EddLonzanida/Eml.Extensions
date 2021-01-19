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
        private readonly Func<T, T, int> lambdaComparer;

        public LambdaComparer(Func<T, T, int> lambdaComparer)
        {
            this.lambdaComparer = lambdaComparer.CheckNotNull();
        }

        public int Compare(T x, T y)
        {
            return lambdaComparer(x, y);
        }
    }
}
