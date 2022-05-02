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
		private readonly Func<T?, T?, int> lambdaComparer;

		public LambdaComparer(Func<T?, T?, int> lambdaComparer)
		{
			this.lambdaComparer = lambdaComparer.CheckNotNull();
		}

		public int Compare(T? x, T? y)
		{
			if (x == null && y == null)
			{
				return (int)eSortFlag.Equal;
			}
			else if (x != null && y == null)
			{
				return (int)eSortFlag.GreaterThan;
			}
			else if (x == null && y != null)
			{
				return (int)eSortFlag.LessThan;
			}

			return lambdaComparer(x, y);
		}
	}
}
