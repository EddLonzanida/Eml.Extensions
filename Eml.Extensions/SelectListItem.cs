namespace Eml.Extensions;

public class SelectListItem
{
    public bool? Selected { get; set; }

    public required string Text { get; set; }

    public required string Value { get; set; }

    public string? SubGroup { get; set; }

    public string? SubGroup2 { get; set; }
}
