namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SportsbookBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private SportsbookBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<SportsbookBetSettingDataAccess>();
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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelEditCustomer")).Returns(_objectDb);
            var custId = 1;
            var esportSetting = EsportSettingType.LicenseeOffering;

            // Action
            _dataAccess.GetBetSetting(custId, ref esportSetting);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_All")).Returns(_objectDb);
            var data = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "::1",
                Language = "LanguageTest"
            };
            var setting = new Dictionary<string, object> { { "UpdateMinMaxBet", true } };

            // Action
            _dataAccess.UpdateBetSetting(data, setting);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, data.LicenseeId);
            _objectDb.Received().AddInParameter("ip", DbType.String, data.Ip);
            _objectDb.Received().AddInParameter("language", DbType.String, data.Language);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, data.UserId);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, data.CustId);
            _objectDb.Received().AddInParameter("UpdateMinMaxBet", DbType.Boolean, true);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.String, 600);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMember_All")).Returns(_objectDb);
            var data = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "::1",
                Language = "LanguageTest"
            };
            var setting = new Dictionary<string, object> { { "UpdateMinMaxBet", true } };
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            _objectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            _objectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = _dataAccess.UpdateBetSetting(data, setting);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        [Test]
        public void UpdateEsportStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdEsportStatus")).Returns(_objectDb);
            var setting = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "::1",
                Language = "LanguageTest"
            };
            var isDisabled = true;

            // Action
            _dataAccess.UpdateEsportStatus(setting, isDisabled);

            // Assert
            _objectDb.Received().AddInParameter("ip", DbType.String, setting.Ip);
            _objectDb.Received().AddInParameter("Langkey", DbType.String, setting.Language);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, setting.UserId);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, setting.CustId);
            _objectDb.Received().AddInParameter("DisableEsports", DbType.Boolean, isDisabled);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.String, 600);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateEsportStatus_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdEsportStatus")).Returns(_objectDb);
            var setting = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "::1",
                Language = "LanguageTest"
            };
            var isDisabled = true;
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            _objectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            _objectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = _dataAccess.UpdateEsportStatus(setting, isDisabled);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }
    }
}