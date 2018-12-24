using System.Linq;
using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetFull
{
    public class UniqueStringPatternTests
    {
        [Fact]
        public void UniqueString_ShouldContainDistinct()
        {
            var uniqueStringPattern = new UniqueStringPattern(new[] { "Eml.*.dll", "Eml.*.exe" }).Build();

            uniqueStringPattern.Count.ShouldBe(2);
            uniqueStringPattern.First().ShouldBe("Eml.*.dll");
            uniqueStringPattern.Last().ShouldBe("Eml.*.exe");
        }

        [Fact]
        public void UniqueString_ShouldRemoveEmptyStrings()
        {
            var uniqueStringPattern = new UniqueStringPattern(new[] { "Eml.*.dll", "Eml.*.exe", "", null, "AppPrefix*.dll" }).Build();

            uniqueStringPattern.Count.ShouldBe(3);
            uniqueStringPattern.First().ShouldBe("Eml.*.dll");
            uniqueStringPattern.Last().ShouldBe("AppPrefix*.dll");
        }
    }
}
