namespace Fanex.Athena.Extension.UnitTests.Helper
{
    using System;
    using Extension.Helper;
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
            var result = Utility.Util.IsValidLanguage(languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("123456", false)]
        [TestCase("123ab", false)]
        [TestCase("1234aa", true)]
        public void CheckValidPassword_WhenCall_ReturnCorrectResult(string languageCode, bool expected)
        {
            // Action
            var result = Utility.Util.CheckValidPassword(languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
