namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AgCasinoBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private AgCasinoBetSettingDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<AgCasinoBetSettingDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetBetSetting_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_AGC_SelCustSetting")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.GetBetSetting(custId);

            // Assert
            _objectDb.Received(1).AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received(1).AddOutParameter("Currency", DbType.Int32, int.MaxValue);
            _objectDb.Received(1).AddOutParameter("AGGroup", DbType.String, 20);
            _objectDb.Received(1).AddOutParameter("IsEnabled", DbType.Boolean, 1);
            _objectDb.Received(1).AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
            _objectDb.Received(1).AddOutParameter("CheckedGroups", DbType.String, 500);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_AGC_UpdCustSetting")).Returns(_objectDb);
            var group = "GroupTest";
            var data = new CommonBetSetting
            {
                Ip = "IP",
                Language = "LanguageTest",
                UserId = 1,
                CustId = 2
            };

            // Action
            _dataAccess.UpdateBetSetting(data, group);

            // Assert
            _objectDb.Received(1).AddInParameter("aggroup", DbType.String, group);
            _objectDb.Received(1).AddInParameter("ip", DbType.String, data.Ip);
            _objectDb.Received(1).AddInParameter("language", DbType.String, data.Language);
            _objectDb.Received(1).AddInParameter("userid", DbType.Int32, data.UserId);
            _objectDb.Received(1).AddInParameter("custid", DbType.Int32, data.CustId);
            _objectDb.Received(1).AddOutParameter("ErrCode", DbType.String, 600);
            _objectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateStatus_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_AGC_UpdCustStatus")).Returns(_objectDb);
            var userId = 1;
            var custId = 2;
            var language = "LanguageTest";
            var ip = "IP";
            var isEnabled = true;

            // Action
            _dataAccess.UpdateStatus(userId, custId, language, ip, isEnabled);

            // Assert
            _objectDb.Received(1).AddInParameter("isenabled", DbType.Boolean, isEnabled);
            _objectDb.Received(1).AddInParameter("ip", DbType.String, ip);
            _objectDb.Received(1).AddInParameter("language", DbType.String, language);
            _objectDb.Received(1).AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received(1).AddInParameter("custid", DbType.Int32, custId);

            _objectDb.Received(1).AddOutParameter("ErrCode", DbType.String, 600);
            _objectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 8000);
        }
    }
}