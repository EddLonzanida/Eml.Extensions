using System;
using System.Collections.Generic;
using System.Linq;

namespace Eml.Extensions
{
    public sealed class UniqueStringPattern : IDisposable
    {
        private List<string> Patterns { get; } = new List<string>();

        public UniqueStringPattern(IReadOnlyCollection<string> patterns)
        {
            if (patterns == null) return;

            Patterns = patterns.ToList()
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList()
                .ConvertAll(r => r.Trim());

            if (!Patterns.Any()) throw new Exception("Pattern is required.");

            Patterns.Sort();
        }

        public List<string> Build()
        {
            var assemblyPatterns = new List<string>();

            if (!Patterns.Any()) return assemblyPatterns;

            assemblyPatterns.AddRange(Patterns);

            var itemsToRemove = assemblyPatterns;
            var results = assemblyPatterns.Except(itemsToRemove, (x, y) => x != y && y.ToLower().StartsWith(x.ToLower()));

            return results.Distinct().ToList();
        }

        public void Dispose()
        {
            Patterns?.Clear();
        }
    }
}
