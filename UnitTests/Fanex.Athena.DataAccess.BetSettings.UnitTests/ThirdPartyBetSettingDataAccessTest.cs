namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ThirdPartyBetSettingDataAccessTest : BaseDataAccessHelperTest<ThirdPartyBetSettingDataAccess>
    {
        private IObjectDb _objectDb;
        private ThirdPartyBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ThirdPartyBetSettingDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductSelCustStatus")).Returns(ObjectDb);
            var custId = 1;
            var productId = 2;

            // Action
            DataAccess.GetBetSetting(custId, productId);

            // Assert
            _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received(1).AddInParameter("ProductId", DbType.Int32, productId);
            _objectDb.Received(1).AddOutParameter("IsEnabled", DbType.Boolean, 1);
            _objectDb.Received(1).AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
            _objectDb.Received(1).AddOutParameter("Currency", DbType.Int32, 1);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductSelCustStatus")).Returns(ObjectDb);
            var custId = 1;
            var productId = 2;
            var expected = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = false,
                CurrencyId = 1
            };

            ObjectDb.GetParameterValue("IsEnabled").Returns(expected.IsEnabled);
            ObjectDb.GetParameterValue("IsLineEnabled").Returns(expected.IsLineEnabled);
            ObjectDb.GetParameterValue("Currency").Returns(expected.CurrencyId);

            // Action
            var result = DataAccess.GetBetSetting(custId, productId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(expected.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(expected.IsLineEnabled));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
        }

        [Test]
        public void UpdateStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductUpdCustStatus")).Returns(ObjectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "IP";
            var isEnabled = true;

            // Action
            DataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            _objectDb.Received(1).AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received(1).AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received(1).AddInParameter("productid", DbType.Int32, productId);
            _objectDb.Received(1).AddInParameter("isenabled", DbType.Boolean, isEnabled);
            _objectDb.Received(1).AddInParameter("ip", DbType.String, ip);
            _objectDb.Received(1).AddInParameter("language", DbType.String, language);
            _objectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductUpdCustStatus")).Returns(ObjectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "IP";
            var isEnabled = true;

            var expected = new ResultInfo
            {
                Code = 0,
                Message = "Success"
            };
            ObjectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = DataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }
    }
}