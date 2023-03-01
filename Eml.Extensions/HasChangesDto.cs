namespace Eml.Extensions;

/// <summary>
///     Used by <see cref="TypeExtensions.HasChanges{T}(T,T,List{string}?)" />
/// </summary>
public class HasChangesDto
{
    public string PropertyName { get; set; } = string.Empty;

    public object? ExistingValue { get; set; }

    public object? IncomingValue { get; set; }
}
