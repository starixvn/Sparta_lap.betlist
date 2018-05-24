namespace Fanex.Athena.Extension.UnitTests.Helper
{
    using Extension.Helper;
    using NUnit.Framework;

    public class CasinoHelperTest
    {
        [TestCase("zh-CN", "_Cs")]
        [TestCase("zh-Hans", "_Cs")]
        [TestCase("zh-TW", "_Ct")]
        [TestCase("ko-KR", "_Ko")]
        [TestCase("default", "_En")]
        public void GetColumnNameByLanguage_WhenCall_ReturnCorrectsuffix(string languageCode, string expectedSuffix)
        {
            // Action
            var result = CasinoHelper.GetColumnNameByLanguage(string.Empty, languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectedSuffix));
        }
    }
}