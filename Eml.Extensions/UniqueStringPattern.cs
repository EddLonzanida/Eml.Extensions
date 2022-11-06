namespace Eml.Extensions;

public sealed class UniqueStringPattern : IDisposable
{
    private List<string> Patterns { get; } = new();

    public UniqueStringPattern(IReadOnlyCollection<string>? patterns)
    {
        if (patterns == null)
        {
            return;
        }

        Patterns = patterns.ToList()
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .ToList()
            .ConvertAll(r => r.Trim());

        if (!Patterns.Any())
        {
            throw new Exception("Pattern is required.");
        }

        Patterns.Sort();
    }

    public void Dispose()
    {
        Patterns?.Clear();
    }

    public List<string> Build()
    {
        var assemblyPatterns = new List<string>();

        if (!Patterns.Any())
        {
            return assemblyPatterns;
        }

        assemblyPatterns.AddRange(Patterns);

        var results = assemblyPatterns.Except(assemblyPatterns, (x, y) => x != y
                                                                          && (y?.ToLower() ?? string.Empty)
                                                                          .StartsWith(x?.ToLower() ?? string.Empty));

        return results.Distinct().ToList();
    }
}
