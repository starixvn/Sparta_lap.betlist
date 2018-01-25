namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreBetSettingDataAccessTest
    {
        [Test]
        public void GetBetSettingProducts_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<CoreBetSettingDataAccess>();
            dataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeProductSel")).Returns(objectDb);
            var licenseeId = 1;
            var expected = new BetSettingProduct { ProductId = 1, IsEnable = true, ProductCode = "CODE" };
            objectDb.Query<BetSettingProduct>(Arg.Any<object>()).Returns(new List<BetSettingProduct> { expected });

            // Action
            var result = dataAccess.GetBetSettingProducts(licenseeId).ElementAt(0);

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