namespace Fanex.Athena.Extension.UnitTests.Utility
{
    using System.Linq;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class GroupCommSettingTest
    {
        [Test]
        public void IsAllValid_WhenCall_ReturnCorrectValue()
        {
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(GroupCommSetting.IsAllValid(0, 1, 2), Is.False);
                Assert.That(GroupCommSetting.IsAllValid(0, 0.0005m, 0.001m, 0.15m), Is.True);
            });
        }
    }
}