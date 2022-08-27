using Shouldly;
using System.Linq;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore;

public class UniqueStringPatternTests
{
    [Fact]
    public void UniqueString_ShouldContainOneEml()
    {
        var uniqueStringPattern = new UniqueStringPattern(new[] { "Eml", "Eml." }).Build();

        uniqueStringPattern.Count.ShouldBe(1);
        uniqueStringPattern.First().ShouldBe("Eml");
    }

    [Fact]
    public void UniqueString_ShouldContainDistinct()
    {
        var uniqueStringPattern = new UniqueStringPattern(new[] { "Eml", "Eml." }).Build();

        uniqueStringPattern.Count.ShouldBe(1);
        uniqueStringPattern.First().ShouldBe("Eml");
    }

    [Fact]
    public void UniqueString_ShouldRemoveEmptyStrings()
    {
        var uniqueStringPattern = new UniqueStringPattern(new[] { "Eml", "", null, "AppPrefix" }).Build();

        uniqueStringPattern.Count.ShouldBe(2);
        uniqueStringPattern.First().ShouldBe("AppPrefix");
        uniqueStringPattern.Last().ShouldBe("Eml");
    }

    [Fact]
    public void UniqueString_ShouldReturnEmlSolutions()
    {
        var uniqueStringPattern = new UniqueStringPattern(new[] { "EmlSolutions." }).Build();

        uniqueStringPattern.Count.ShouldBe(1);
        uniqueStringPattern.First().ShouldBe("EmlSolutions.");
    }
}
