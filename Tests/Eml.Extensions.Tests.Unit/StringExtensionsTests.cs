using Shouldly;
using Xunit;

namespace Eml.Extensions.Tests.Unit
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
    }
}
