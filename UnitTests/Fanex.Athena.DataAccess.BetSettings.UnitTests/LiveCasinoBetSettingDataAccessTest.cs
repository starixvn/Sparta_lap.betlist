namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.Common.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LiveCasinoBetSettingDataAccessTest : BaseDataAccessHelperTest<LiveCasinoBetSettingDataAccess>
    {
        private ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;

        private IUserLogDataAccess _userLogDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();

            _coreCasinoBetSettingDataAccess = Substitute.For<ICoreCasinoBetSettingDataAccess>();
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            DataAccess = Substitute.ForPartsOf<LiveCasinoBetSettingDataAccess>(_userLogDataAccess, _coreCasinoBetSettingDataAccess);
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>("TP_Admin_StakeLevel_GetCustStakeLevelMulti")).Returns(ObjectDb);
            var custId = 1;
            var language = "LanguageTest";

            // Action
            DataAccess.GetBetSetting(custId, language);

            // Assert
            ObjectDb.Received().AddInParameter("custid", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("TP_Admin_StakeLevel_GetCustStakeLevelMulti")).Returns(ObjectDb);
            var custId = 1;
            var language = "LanguageTest";
            var expected = SetUpForUpdateBetSetting(1, 0, string.Empty);

            // Action
            var result = DataAccess.GetBetSetting(custId, language)[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GameType, Is.EqualTo(expected.GameType));
                Assert.That(result.GameTypeId, Is.EqualTo(expected.GameTypeId));
                Assert.That(result.StakeLevelId, Is.EqualTo(expected.StakeLevelId));
                Assert.That(result.StakeLevelName, Is.EqualTo(expected.StakeLevelName));
                Assert.That(result.IsCheck, Is.EqualTo(expected.IsCheck));
            });
        }

        [Test]
        public void UpdateStatus_WhenCall_CallCoreCasinoBetSettingDataAccess()
        {
            // Arrange
            var metaModel = new BetSettingCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 3,
                Language = "Lao",
                Ip = "123"
            };
            const int LiveCasinoSportId = 162;

            // Action
            DataAccess.UpdateStatus(metaModel);

            // Assert
            _coreCasinoBetSettingDataAccess.Received().UpdateCasinoStatus(metaModel, LiveCasinoSportId);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);

            var custId = 1;
            var betSetting = new LiveCasinoBetSetting
            {
                GameTypeId = 1,
                StakeLevelId = 2,
            };

            // Action
            DataAccess.UpdateBetSetting(0, custId, betSetting, string.Empty, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("gametypeid", DbType.Int32, betSetting.GameTypeId);
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, custId);
                ObjectDb.Received().AddInParameter("stakelevellist", DbType.Int32, betSetting.StakeLevelId);
                ObjectDb.Received().AddInParameter("roleid", DbType.Int32, 1);
            });
        }

        [Test]
        public void UpdateBetSetting_WhenCall_TrackingValue2Correctly()
        {
            // Arrange
            const string TrackingValue = "<{0} CustID=\"{1}\" GameTypeID=\"{2}\" BetTypeID=\"0\" StakeLevel=\"{3}\" Ip=\"{4}\""
                + " Lang=\"{5}\" RoleID=\"1\" ErrCode=\"0\" ErrMsg=\"\"/>";
            const string UspName = "TP_User_CustGameType_UpdStakeLevel";
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);

            const int CustId = 1;
            const string Language = "lan";
            const string Ip = "::1";

            var betSetting = new LiveCasinoBetSetting { GameTypeId = 1 };
            var liveCasinoBetSetting = SetUpForUpdateBetSetting(betSetting.GameTypeId, 0, string.Empty);

            UserLogMetaModel logData = null;
            _userLogDataAccess.When(x => x.WriteLog(Arg.Any<UserLogMetaModel>())).Do(x => logData = (UserLogMetaModel)x[0]);

            var expectedTrackingValue = string.Format(
                TrackingValue,
                UspName,
                CustId,
                liveCasinoBetSetting.GameTypeId,
                liveCasinoBetSetting.StakeLevelName,
                Ip,
                Language);

            // Action
            DataAccess.UpdateBetSetting(0, CustId, betSetting, Ip, Language);

            // Assert
            Assert.That(logData.TrackingValue2, Is.EqualTo(expectedTrackingValue));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_TrackingValue1Correctly()
        {
            // Arrange
            const string UspName = "TP_User_CustGameType_UpdStakeLevel";
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);

            const int CustId = 1;
            const string Language = "lan";
            const string Ip = "::1";
            const int ErrorCode = 1;
            const string ErrorMrg = "Error";

            var betSetting = new LiveCasinoBetSetting { GameTypeId = 1 };
            var liveCasinoBetSetting = SetUpForUpdateBetSetting(2, ErrorCode, ErrorMrg);

            UserLogMetaModel logData = null;
            _userLogDataAccess.When(x => x.WriteLog(Arg.Any<UserLogMetaModel>())).Do(x => logData = (UserLogMetaModel)x[0]);

            var expectedTrackingValue = $"<{UspName} CustID=\"{CustId}\" GameTypeID=\"{betSetting.GameTypeId}\" BetTypeID=\"0\" StakeLevel=\"\" Ip=\"{Ip}\""
                + $" Lang=\"{Language}\" RoleID=\"1\" ErrCode=\"{ErrorCode}\" ErrMsg=\"{ErrorMrg}\"/>";

            // Action
            DataAccess.UpdateBetSetting(0, CustId, betSetting, Ip, Language);

            // Assert
            Assert.That(logData.TrackingValue1, Is.EqualTo(expectedTrackingValue));
        }

        private LiveCasinoBetSetting SetUpForUpdateBetSetting(int gameTypeId, int errCode, string errMsg)
        {
            var expectedBetSetting = new LiveCasinoBetSetting
            {
                GameTypeId = gameTypeId,
                IsCheck = true,
                GameType = "Casino",
                StakeLevelId = 1,
                StakeLevelName = "Lv1"
            };
            ConfigureReaderForGetBetSettingMethod(expectedBetSetting);

            var resultInfo = new ResultInfo { Code = errCode, Message = errMsg };
            ObjectDb.GetParameterValue("ErrCode").Returns(resultInfo.Code);
            ObjectDb.GetParameterValue("ErrMsg").Returns(resultInfo.Message);

            return expectedBetSetting;
        }

        private void ConfigureReaderForGetBetSettingMethod(LiveCasinoBetSetting expected)
        {
            Reader.Read().Returns(true, false, true, false);

            MapReaderData(new
            {
                GameTypeName_En = expected.GameType,
                GameTypeId = expected.GameTypeId,
                StakeLevelID = expected.StakeLevelId,
                StakeLevelName = expected.StakeLevelName,
                IsChecked = expected.IsCheck
            });
        }
    }
}