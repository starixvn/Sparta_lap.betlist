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
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeProductSel")).Returns(objectDb);
            var licenseeId = 1;
            var expected = new BetSettingProduct();
            objectDb.Query<BetSettingProduct>(Arg.Any<object>()).Returns(new List<BetSettingProduct> { expected });

            // Action
            var result = dataAccess.GetBetSettingProducts(licenseeId).ElementAt(0);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}