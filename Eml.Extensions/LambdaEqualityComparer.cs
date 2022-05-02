namespace Eml.Extensions
{
	/// <summary>
	/// Used by LinqExtensions.
	/// General purpose IEqualityComparer.
	/// </summary>
	public class LambdaEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T?, T?, bool> lambdaComparer;

		private readonly Func<T?, int> lambdaHash;

		/// <summary>
		/// Example: new LambdaEqualityComparer&lt;TSource&gt;(comparer)
		/// <para>Declaration:</para> 
		/// <para>Func&lt;TSource, TSource, bool&gt; comparer</para> 
		/// </summary>
		public LambdaEqualityComparer(Func<T?, T?, bool> lambdaComparer) :
			this(lambdaComparer, o => 0)
		{
		}

		private LambdaEqualityComparer(Func<T?, T?, bool> lambdaComparer, Func<T?, int> lambdaHash)
		{
			this.lambdaComparer = lambdaComparer.CheckNotNull();
			this.lambdaHash = lambdaHash.CheckNotNull();
		}

		public bool Equals(T? x, T? y)
		{
			return lambdaComparer(x, y);
		}

		public int GetHashCode(T obj)
		{
			return lambdaHash(obj);
		}
	}
}