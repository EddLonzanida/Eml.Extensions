using Shouldly;
using System;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore;

public class DateExtensionTests
{
    [Fact]
    public void ToBeforeMidnight_ShouldReturnValidDateBeforeMidnight()
    {
        var sut = new DateTime(2020, 5, 25, 11, 59, 59);
        var expectedResult = new DateTime(sut.Year, sut.Month, sut.Day, 23, 59, 59).ToString("yyyy-MM-dd hh:mm:ss tt");
        var r = sut.ToBeforeMidnight().ToString("yyyy-MM-dd hh:mm:ss tt");

        r.ShouldBe(expectedResult);
    }

    [Fact]
    public void ToBeforeMidnight_ShouldReturnValidNullableDateBeforeMidnight()
    {
        DateTime? sut = new DateTime(2020, 5, 25, 11, 59, 59);
        var expectedResult = new DateTime(sut.Value.Year, sut.Value.Month, sut.Value.Day, 23, 59, 59).ToString("yyyy-MM-dd hh:mm:ss tt");
        var r = sut.ToBeforeMidnight()?.ToString("yyyy-MM-dd hh:mm:ss tt");

        r.ShouldBe(expectedResult);
    }
}
