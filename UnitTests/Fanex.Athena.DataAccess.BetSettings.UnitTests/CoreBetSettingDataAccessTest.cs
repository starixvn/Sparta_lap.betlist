namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
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

        [Test]
        public void IsBetSettingEditable_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            reader["IsGrant"].Returns(true);
            var objectDb = Substitute.For<IObjectDb>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            var dataAccess = Substitute.ForPartsOf<CoreBetSettingDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckEditBetSetting")).Returns(objectDb);
            
            // Action
            var result = dataAccess.IsBetSettingEditable(0, 0);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}