namespace Fanex.Athena.Extension.UnitTests.Helper
{
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class UtilTest
    {
        [TestCase("en-US", true)]
        [TestCase("zh-TW", true)]
        [TestCase("zh-CN", true)]
        [TestCase("ko-KR", true)]
        [TestCase("zh-Hans", true)]
        [TestCase("en-us", false)]
        [TestCase("vi-VN", false)]
        public void IsValidLanguage_WhenCall_ReturnCorrectResult(string languageCode, bool expected)
        {
            // Action
            var result = Util.IsValidLanguage(languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("123456", false)]
        [TestCase("123a456", false)]
        [TestCase("1abcdef", false)]
        [TestCase("123ab", false)]
        [TestCase("123ab$", true)]
        [TestCase("1234aa", true)]
        [TestCase("1234Aa", true)]
        [TestCase("1a2b3c", true)]
        public void IsPasswordValid_WhenCall_ReturnCorrectResult(string languageCode, bool expected)
        {
            // Action
            var result = Util.IsPasswordValid(languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}