namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ThirdPartyBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private ThirdPartyBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ThirdPartyBetSettingDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_3rdProductSelCustStatus")).Returns(_objectDb);
            var custId = 1;
            var productId = 2;

            // Action
            _dataAccess.GetBetSetting(custId, productId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("ProductId", DbType.Int32, productId);
            _objectDb.Received().AddOutParameter("IsEnabled", DbType.Boolean, 1);
            _objectDb.Received().AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
            _objectDb.Received().AddOutParameter("Currency", DbType.Int32, 1);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_3rdProductSelCustStatus")).Returns(_objectDb);
            var custId = 1;
            var productId = 2;
            var expected = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = false,
                CurrencyId = 1
            };

            _objectDb.GetParameterValue("IsEnabled").Returns(expected.IsEnabled);
            _objectDb.GetParameterValue("IsLineEnabled").Returns(expected.IsLineEnabled);
            _objectDb.GetParameterValue("Currency").Returns(expected.CurrencyId);

            // Action
            var result = _dataAccess.GetBetSetting(custId, productId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(expected.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(expected.IsLineEnabled));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
        }

        [Test]
        public void UpdateStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_3rdProductUpdCustStatus")).Returns(_objectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "::1";
            var isEnabled = true;

            // Action
            _dataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("productid", DbType.Int32, productId);
            _objectDb.Received().AddInParameter("isenabled", DbType.Boolean, isEnabled);
            _objectDb.Received().AddInParameter("ip", DbType.String, ip);
            _objectDb.Received().AddInParameter("language", DbType.String, language);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_3rdProductUpdCustStatus")).Returns(_objectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "::1";
            var isEnabled = true;

            var expected = new ResultInfo
            {
                Code = 0,
                Message = "Success"
            };
            _objectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = _dataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }
    }
}