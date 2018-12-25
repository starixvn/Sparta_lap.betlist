namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreBetSettingDataAccessTest : BaseDataAccessHelperTest<CoreBetSettingDataAccess>
    {
        [Test]
        public void GetBetSettingProducts_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeProductSel")).Returns(ObjectDb);
            var licenseeId = 1;
            var expected = new BetSettingProduct { ProductId = 1, IsEnable = true, ProductCode = "CODE" };
            ObjectDb.Query<BetSettingProduct>(Arg.Any<object>()).Returns(new List<BetSettingProduct> { expected });

            // Action
            var result = DataAccess.GetBetSettingProducts(licenseeId).ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ProductId, Is.EqualTo(expected.ProductId));
                Assert.That(result.IsEnable, Is.EqualTo(expected.IsEnable));
                Assert.That(result.ProductCode, Is.EqualTo(expected.ProductCode));
                Assert.That(result.ProductName, Is.EqualTo(expected.ProductName));
            });
        }
    }
}