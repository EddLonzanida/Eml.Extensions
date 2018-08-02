using System;
using System.Collections.Generic;

namespace Eml.Extensions
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaComparer;

        private readonly Func<T, int> _lambdaHash;

        public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer) :
            this(lambdaComparer, o => 0)
        {
        }

        private LambdaEqualityComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            _lambdaComparer = lambdaComparer.CheckNotNull(nameof(lambdaComparer));
            _lambdaHash = lambdaHash.CheckNotNull(nameof(lambdaHash));
        }

        public bool Equals(T x, T y)
        {
            return _lambdaComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _lambdaHash(obj);
        }
    }
}