namespace Eml.Extensions;

/// <summary>
///     Used by LinqExtensions.
///     General purpose IEqualityComparer.
/// </summary>
public class LambdaEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> lambdaComparer;

    private readonly Func<T, int> lambdaHash;

    /// <summary>
    ///     Example:
    ///     <code language="c#">new LambdaEqualityComparer<TSource></TSource>(<paramref name="lambdaComparer" />)</code>
    ///     <para>Declaration:</para>
    ///     <code language="c#">Func&lt;TSource, TSource, bool&gt; <paramref name="lambdaComparer" /></code>
    /// </summary>
    public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer) :
        this(lambdaComparer, o => 0)
    {
    }

    private LambdaEqualityComparer(Func<T, T, bool> lambdaComparer,
        Func<T, int> lambdaHash)
    {
        this.lambdaComparer = lambdaComparer.CheckNotNull();
        this.lambdaHash = lambdaHash.CheckNotNull();
    }

    public bool Equals(T? x, T? y)
    {
        return y != null && x != null && lambdaComparer(x, y);
    }

    public int GetHashCode(T obj)
    {
        return lambdaHash(obj);
    }
}
