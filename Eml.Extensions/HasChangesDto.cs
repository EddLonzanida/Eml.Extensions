namespace Eml.Extensions;

/// <summary>
///     Used by <see cref="TypeExtensions.HasChanges{T}(T,T,List{string}?)" />
/// </summary>
public class HasChangesDto
{
    public string PropertyName { get; set; } = string.Empty;

    public object? Value1 { get; set; }

    public object? Value2 { get; set; }
}
