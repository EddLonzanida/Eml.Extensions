using Assembly1;
using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Integration.NetCore
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("first1", "First1", '.')]
        [InlineData("first2.second", "First2.Second", '.')]
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
        public void ToProperCase_ShouldCapitalizeFirstLetters(string sut, string expectedResult, char delimeter)
        {
            sut.ToProperCase(delimeter).ShouldBe(expectedResult);
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
        public void ToSpaceDelimitedWords_ShouldDelimitedWords(string sut, string expectedResult)
        {
            sut.ToSpaceDelimitedWords().ShouldBe(expectedResult);
        }

        [Fact]
        public void TrimStringProperties_ShouldTrimProperties()
        {
            const string LAST_NAME = "Last Name";
            const string FIRST_NAME = "First Name";

            var sut = new TestClass { FirstName = $"  {FIRST_NAME} ", LastName = $" {LAST_NAME} " };

            sut.TrimStringProperties();

            sut.FirstName.ShouldBe(FIRST_NAME);
            sut.LastName.ShouldBe(LAST_NAME);
        }

        [Theory]
        [InlineData("company", "companies")]
        [InlineData("COMPANY", "COMPANIES")]
        [InlineData("COMPANy", "COMPANies")]
        [InlineData("COMPANies", "COMPANies")]
        [InlineData("Title", "Titles")]
        [InlineData("TITLE", "TITLES")]
        [InlineData("Tab", "Tabs")]
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
        public void TrimRight_ShouldRemoveLastString(string sut, string trim, string expectedResult)
        {
            sut.TrimRight(trim).ShouldBe(expectedResult);
        }
    }
}
