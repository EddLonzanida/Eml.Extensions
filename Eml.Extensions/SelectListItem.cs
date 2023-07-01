namespace Eml.Extensions;

public class SelectListItem
{
    public bool? Selected { get; set; }

#if NET6
    public string Text { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
#endif
#if NET7
    public required string Text { get; set; }

    public required string Value { get; set; }
#endif
    public string? SubGroup { get; set; }

    public string? SubGroup2 { get; set; }
}
