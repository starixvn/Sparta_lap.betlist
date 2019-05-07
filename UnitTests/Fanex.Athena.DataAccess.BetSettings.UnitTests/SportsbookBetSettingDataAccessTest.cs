namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Fanex.Athena.Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Utility;

    [TestFixture]
    public class SportsbookBetSettingDataAccessTest : BaseDataAccessHelperTest<SportsbookBetSettingDataAccess>
    {
        private IAppSettings _appSetting;

        protected override void SetUpDataAccess()
        {
            var betSettingReader = Substitute.For<IBetSettingReader>();
            betSettingReader.ReadForSport(Arg.Any<IDataReader>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(x => new BetSetting
                {
                    SportId = x[1].ToString()
                });
            _appSetting = Substitute.For<IAppSettings>();
            DataAccess = Substitute.ForPartsOf<SportsbookBetSettingDataAccess>(
                _appSetting,
                betSettingReader);
        }

        [Test]
        public async Task GetBetSettingAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            const int CustId = 1;

            // Action
            await DataAccess.GetBetSettingAsync(CustId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("CustID", CustId);
        }

        [Test]
        public async Task GetBetSettingAsync_ReaderNotHaveData_ReturnDefaultResult()
        {
            // Arrange
            _appSetting.IsShowMixedSport.Returns(true);

            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            Reader.Read().Returns(false);

            // Action
            var result = await DataAccess.GetBetSettingAsync(Arg.Any<int>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainData.Any(), Is.False);
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetBetSettingAsync_WhenCall_DataReturnCorrectly(bool isShowMixedSport)
        {
            // Arrange
            _appSetting.IsShowMixedSport.Returns(isShowMixedSport);

            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditCustomer")).Returns(ObjectDb);
            Reader.Read().Returns(true);
            Reader.FieldCount.Returns(1);
            Reader[Arg.Any<string>()].Returns(string.Empty);

            const int CustId = 1;
            var expectedSportIds = "1,2,3,5,8,10,11,99,99MP";
            if (!isShowMixedSport)
            {
                expectedSportIds = expectedSportIds.Replace(",99MP", string.Empty);
            }

            // Action
            var result = await DataAccess.GetBetSettingAsync(CustId);
            var sportIdList = string.Join(",", result.MainData.Select(x => x.SportId));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sportIdList, Is.EqualTo(expectedSportIds));
            });
        }

        [TestCase("UpdateMinMaxBet", true)]
        [TestCase("Test", 1)]
        public async Task UpdateBetSettingAsync_WhenCall_ParametersPassedCorrectly(string paramterName, object parameterValue)
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
            await DataAccess.UpdateBetSettingAsync(data, setting);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("licenseeid", data.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("ip", data.Ip);
                ObjectDb.Received(1).SetParameterValue("language", data.Language);
                ObjectDb.Received(1).SetParameterValue("userid", data.UserId);
                ObjectDb.Received(1).SetParameterValue("custid", data.CustId);
                ObjectDb.Received(1).SetParameterValue(paramterName, parameterValue);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_DataReturnedCorrectly()
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
            var result = await DataAccess.UpdateBetSettingAsync(data, setting);

            // Assert
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expected.Code));
                Assert.That(result.Message, Is.EqualTo(expected.Message));
            });
        }
    }
}