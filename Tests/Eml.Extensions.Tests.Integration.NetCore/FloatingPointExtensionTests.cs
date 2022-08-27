using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore;

public class FloatingPointExtensionTests
{
    [Theory]
    [InlineData(0.000006)]
    [InlineData(0.000001)]
    [InlineData(0.000009)]
    public void WhenDouble_IsZero_ShouldBeTrue(double number)
    {
        var sut = number.IsZero();

        sut.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.00001)]
    [InlineData(0.0004)]
    [InlineData(0.00005)]
    [InlineData(0.00009)]
    [InlineData(1.0004)]
    [InlineData(1.00005)]
    public void WhenDouble_IsZero_ShouldBeFalse(double number)
    {
        var sut = number.IsZero();

        sut.ShouldBeFalse();
    }

    [Theory]
    [InlineData(0.000006)]
    [InlineData(0.000001)]
    [InlineData(0.000009)]
    public void WhenFloat_IsZero_ShouldBeTrue(float number)
    {
        var sut = number.IsZero();

        sut.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.00001)]
    [InlineData(0.0004)]
    [InlineData(0.00005)]
    [InlineData(0.00009)]
    [InlineData(1.0004)]
    [InlineData(1.00005)]
    public void WhenFloat_IsZero_ShouldBeFalse(float number)
    {
        var sut = number.IsZero();

        sut.ShouldBeFalse();
    }

    [Theory]
    [InlineData(0.000006)]
    [InlineData(0.000001)]
    [InlineData(0.000009)]
    public void WhenDecimal_IsZero_ShouldBeTrue(decimal number)
    {
        var sut = number.IsZero();

        sut.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0.00001)]
    [InlineData(0.0004)]
    [InlineData(0.00005)]
    [InlineData(0.00009)]
    [InlineData(1.0004)]
    [InlineData(1.00005)]
    public void WhenDecimal_IsZero_ShouldBeFalse(decimal number)
    {
        var sut = number.IsZero();

        sut.ShouldBeFalse();
    }
}
