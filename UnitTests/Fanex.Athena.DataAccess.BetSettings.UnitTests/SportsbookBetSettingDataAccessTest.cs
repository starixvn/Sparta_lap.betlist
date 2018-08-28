namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Utility;

    [TestFixture]
    public class SportsbookBetSettingDataAccessTest : BaseDataAccessHelperTest<SportsbookBetSettingDataAccess>
    {
        private IAppSettings _appSetting;
        
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _appSetting = Substitute.For<IAppSettings>();
            var betSettingReader = Substitute.For<IBetSettingReader>();
            betSettingReader.ReadForSport(Arg.Any<IDataReader>(), Arg.Any<string>(), Arg.Any<bool>()).
                Returns(x => new BetSetting
                {
                    SportId = x[1].ToString()
                });
            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<SportsbookBetSettingDataAccess>(_appSetting, betSettingReader);
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            const int CustId = 1;

            // Action
            DataAccess.GetBetSetting(CustId);

            // Assert
            ObjectDb.Received().AddInParameter("CustID", DbType.Int32, CustId);
        }

        [Test]
        public void GetBetSetting_ReaderNotHaveData_ReturnDefaultResult()
        {
            // Arrange
            _appSetting.IsShowMixedSport.Returns(true);

            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            Reader.Read().Returns(false);

            // Action
            var result = DataAccess.GetBetSetting(Arg.Any<int>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainData.Any(), Is.False);
                Assert.That(result.EsportSetting, Is.EqualTo(EsportSettingType.NoneOffering));
            });
        }

        [TestCase(false, EsportSettingType.NoneOffering, true)]
        [TestCase(true, EsportSettingType.LicenseeOffering, false)]
        [TestCase(true, EsportSettingType.NoneMemberOffering, true)]
        public void GetBetSetting_WhenCall_DataReturnCorrectly(bool hasDisableEsportsColumn, EsportSettingType expectedEsportSetting, bool isShowMixedSport)
        {
            // Arrange
            _appSetting.IsShowMixedSport.Returns(isShowMixedSport);

            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            Reader.Read().Returns(true);
            Reader.FieldCount.Returns(1);
            Reader[Arg.Any<string>()].Returns(string.Empty);
            Reader["DisableEsports"].Returns(expectedEsportSetting);
            if (hasDisableEsportsColumn)
            {
                Reader.GetName(Arg.Any<int>()).Returns("DisableEsports");
            }

            const int CustId = 1;
            var expectedSportIds = "1,2,3,5,8,10,11,99,99MP,154";
            if (!isShowMixedSport)
            {
                expectedSportIds = expectedSportIds.Replace(",99MP", string.Empty);
            }

            // Action
            var result = DataAccess.GetBetSetting(CustId);
            var sportIdList = string.Join(",", result.MainData.Select(x => x.SportId));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sportIdList, Is.EqualTo(expectedSportIds));
                Assert.That(result.EsportSetting, Is.EqualTo(expectedEsportSetting));
            });
        }

        [TestCase("UpdateMinMaxBet", true, DbType.Boolean)]
        [TestCase("Test", 1, DbType.Decimal)]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly(string paramterName, object parameterValue, DbType expectedType)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_All")).Returns(ObjectDb);
            var data = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "IP",
                Language = "LanguageTest"
            };
            var setting = new Dictionary<string, object> { { paramterName, parameterValue } };

            // Action
            DataAccess.UpdateBetSetting(data, setting);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("licenseeid", DbType.Int32, data.LicenseeId);
                ObjectDb.Received().AddInParameter("ip", DbType.String, data.Ip);
                ObjectDb.Received().AddInParameter("language", DbType.String, data.Language);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, data.UserId);
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, data.CustId);
                ObjectDb.Received().AddInParameter(paramterName, expectedType, parameterValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.String, 600);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
            });
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_All")).Returns(ObjectDb);
            var data = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "IP",
                Language = "LanguageTest"
            };
            var setting = new Dictionary<string, object> { { "UpdateMinMaxBet", true } };
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            StubGetResultInfo(expected.Code, expected.Message);

            // Action
            var result = DataAccess.UpdateBetSetting(data, setting);

            // Assert
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expected.Code));
                Assert.That(result.Message, Is.EqualTo(expected.Message));
            });
        }

        [Test]
        public void UpdateEsportStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdEsportStatus")).Returns(ObjectDb);
            var setting = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "IP",
                Language = "LanguageTest"
            };
            const bool IsDisabled = true;

            // Action
            DataAccess.UpdateEsportStatus(setting, IsDisabled);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("ip", DbType.String, setting.Ip);
                ObjectDb.Received().AddInParameter("Langkey", DbType.String, setting.Language);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, setting.UserId);
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, setting.CustId);
                ObjectDb.Received().AddInParameter("DisableEsports", DbType.Boolean, IsDisabled);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.String, 600);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
            });
        }

        [Test]
        public void UpdateEsportStatus_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdEsportStatus")).Returns(ObjectDb);
            var setting = new CommonBetSetting
            {
                LicenseeId = 1,
                UserId = 2,
                CustId = 3,
                Ip = "IP",
                Language = "LanguageTest"
            };
            var isDisabled = true;
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            StubGetResultInfo(expected.Code, expected.Message);

            // Action
            var result = DataAccess.UpdateEsportStatus(setting, isDisabled);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expected.Code));
                Assert.That(result.Message, Is.EqualTo(expected.Message));
            });
        }
    }
}