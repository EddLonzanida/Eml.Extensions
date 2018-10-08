using System;
using System.Collections.Generic;
using System.Linq;

namespace Eml.Extensions
{
    public sealed class UniqueStringPattern : IDisposable
    {
        private const string DEFAULT_PATTERN = "Eml.";

        private List<string> Patterns { get; } = new List<string>();

        public UniqueStringPattern(IReadOnlyCollection<string> patterns)
        {
            if (patterns == null) return;

            Patterns = patterns.ToList()
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList()
                .ConvertAll(r => r.Trim());

            Patterns.Sort();
        }

        public List<string> Build()
        {
            var assemblyPatterns = GetDefaultPatterns();

            if (!Patterns.Any()) return assemblyPatterns;

            assemblyPatterns.AddRange(Patterns);

            var itemsToRemove = assemblyPatterns;
            var results = assemblyPatterns.Except(itemsToRemove, (x, y) => x != y && y.ToLower().StartsWith(x.ToLower()));

            return results.Distinct().ToList();
        }

        private List<string> GetDefaultPatterns()
        {
#if NETFULL
            return new List<string>(new[] { $"{DEFAULT_PATTERN}*.dll", $"{DEFAULT_PATTERN}*.exe" });
#endif
#if NETCORE
            return new List<string>(new[] { DEFAULT_PATTERN });
#endif
        }

        public void Dispose()
        {
            Patterns.Clear();
        }
    }
}
