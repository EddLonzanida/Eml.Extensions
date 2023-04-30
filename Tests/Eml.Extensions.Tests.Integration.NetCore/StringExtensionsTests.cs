using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("FIRST1", "First1", '.')]
    [InlineData("first1", "First1", '.')]
    [InlineData("first2.second", "First2.Second", '.')]
    [InlineData("FIRST2.SECOND", "First2.Second", '.')]
    [InlineData("first3..second", "First3..Second", '.')]
    [InlineData(".first4..second", ".First4..Second", '.')]
    [InlineData(".first5..second.", ".First5..Second.", '.')]
    [InlineData(".first6..second..", ".First6..Second..", '.')]
    [InlineData("..first7..second..", "..First7..Second..", '.')]
    [InlineData("first3...second", "First3...Second", '.')]
    [InlineData(".first4...second", ".First4...Second", '.')]
    [InlineData(".first5...second.", ".First5...Second.", '.')]
    [InlineData(".first6...second..", ".First6...Second..", '.')]
    [InlineData("...first7...second..", "...First7...Second..", '.')]
    [InlineData("...first7...second...", "...First7...Second...", '.')]
    public void ToProperCase_ShouldCapitalizeFirstLetters(string sut, string expectedResult, char delimiter)
    {
        sut.ToProperCase(delimiter).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("one", "One")]
    [InlineData("One", "One")]
    [InlineData("OneTwo", "One Two")]
    [InlineData("oneTwo", "One Two")]
    [InlineData("One Two", "One Two")]
    [InlineData("one Two", "One Two")]
    [InlineData("OneTwoThree", "One Two Three")]
    [InlineData("oneTwothree", "One Twothree")]
    [InlineData("One Two Three", "One Two Three")]
    [InlineData("one Two three", "One Two Three")]
    [InlineData(" one  Two       three ", "One Two Three")]
    [InlineData("FulfillmentJobEnabled", "Fulfillment Job Enabled")]
    [InlineData("GFDMS", "GFDMS")]
    [InlineData("FHLMC GFDMS", "FHLMC GFDMS")]
    [InlineData("MortgageSchedule", "Mortgage Schedule")]
    [InlineData("WholeLoan", "Whole Loan")]
    public void ToSpaceDelimitedWords_ShouldDelimitedWords(string sut, string expectedResult)
    {
        sut.ToSpaceDelimitedWords().ShouldBe(expectedResult);
    }

    //[Fact]
    //public void TrimStringProperties_ShouldTrimProperties()
    //{
    //    const string LAST_NAME = "Last Name";
    //    const string FIRST_NAME = "First Name";

    //    var sut = new TestClass { FirstName = $"  {FIRST_NAME} ", LastName = $" {LAST_NAME} " };

    //    sut.TrimStringProperties();

    //    sut.FirstName.ShouldBe(FIRST_NAME);
    //    sut.LastName.ShouldBe(LAST_NAME);
    //}

    [Theory]
    [InlineData("company", "companies")]
    [InlineData("COMPANY", "COMPANIES")]
    [InlineData("COMPANy", "COMPANies")]
    [InlineData("COMPANies", "COMPANies")]
    [InlineData("Title", "Titles")]
    [InlineData("TITLE", "TITLES")]
    [InlineData("Tab", "Tabs")]
    [InlineData("Tabs", "Tabs")]
    [InlineData("man", "men")]
    [InlineData("woman", "women")]
    [InlineData("child", "children")]
    [InlineData("children", "children")]
    [InlineData("tooth", "teeth")]
    [InlineData("foot", "feet")]
    [InlineData("mouse", "mice")]
    [InlineData("belief", "beliefs")]
    public void Pluralize_ShouldPluralize(string sut, string expectedResult)
    {
        sut.Pluralize().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("Generic`1", "`1", "Generic")]
    [InlineData("Generic`1`1", "`1", "Generic")]
    [InlineData("IntellisenseCountConfigParser", "Config", "IntellisenseCount")]
    [InlineData("IntellisenseCountConfigParser", "config", "IntellisenseCount")]
    [InlineData("IntellisenseCountConfigParser", "ConfigParser", "IntellisenseCount")]
    [InlineData("ClassDataNameSpaceClassData", "ClassData", "ClassDataNameSpace")]
    public void TrimRight_ShouldRemoveLastString(string sut, string trim, string expectedResult)
    {
        sut.TrimRight(trim).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("pabbly_payment_success.json", "pabbly_", "payment_success.json")]
    [InlineData("TxnEntityIntBase", "TxnEntity", "IntBase")]
    public void TrimLeft_ShouldRemoveStartString(string sut, string trim, string expectedResult)
    {
        sut.TrimLeft(trim).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("pabbly_payment_success.json", "pabbly_", ".json", "payment_success")]
    [InlineData("EntityIntBase", "TxnEntity", "Base", "EntityInt")]
    public void TrimLeft_ShouldTrimLeftTrimRight(string sut, string trimLeft, string trimRight, string expectedResult)
    {
        sut.TrimLeft(trimLeft).TrimRight(trimRight).ShouldBe(expectedResult);
    }
}
