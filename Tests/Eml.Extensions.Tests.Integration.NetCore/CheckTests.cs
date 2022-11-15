using Shouldly;
using System;
using Xunit;
// ReSharper disable ExpressionIsAlwaysNull

namespace Eml.Extensions.Tests.Integration.NetCore;

public class CheckTests
{
    [Fact]
    public void CheckNotNull_ShouldThrowError()
    {
        SelectListItem sut = null;

        var exception = Should.Throw<ArgumentNullException>(() => sut.CheckNotNull());

        exception.Message.ShouldBe($"Value cannot be null. (Parameter '{nameof(sut)}')");
    }

    [Fact]
    public void CheckNotEmpty_ShouldThrowError()
    {
        var sut = string.Empty;

        var exception = Should.Throw<ArgumentException>(() => sut.CheckNotEmpty());

        exception.Message.ShouldBe($"The argument '{nameof(sut)}' cannot be null, empty or contain only white space.");
    }
}
