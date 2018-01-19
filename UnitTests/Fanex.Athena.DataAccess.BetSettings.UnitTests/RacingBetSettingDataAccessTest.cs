namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RacingBetSettingDataAccessTest : BaseDataAccessHelperTest<RacingBetSettingDataAccess>
    {
        private static IEnumerable GetUspNameByKey
        {
            get
            {
                yield return new TestCaseData("151_31", "Lrf_CustomerUpdMember_MinMaxBet151_31");
                yield return new TestCaseData("151_41", "Lrf_CustomerUpdMember_MinMaxBet151_41");
                yield return new TestCaseData("151_51", "Lrf_CustomerUpdMember_MinMaxBet151_51");
                yield return new TestCaseData("152_41", "Lrf_CustomerUpdMember_MinMaxBet152_41");
                yield return new TestCaseData("152_51", "Lrf_CustomerUpdMember_MinMaxBet152_51");
                yield return new TestCaseData("153_51", "Lrf_CustomerUpdMember_MinMaxBet153_51");
            }
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditRB")).Returns(ObjectDb);

            const int CustId = 1;

            // Action
            DataAccess.GetBetSetting(CustId);

            // Assert
            ObjectDb.Received().AddInParameter("CustID", DbType.Int32, CustId);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetBetSetting_WhenCall_DataReturnCorrectly(bool isAllowedSaToteCurrency)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditRB")).Returns(ObjectDb);
            DataAccess.GetObjectDb(Arg.Is("Lrf_CheckHRAllowedCurrency")).Returns(ObjectDb);

            ObjectDb.GetParameterValue("allowed").Returns(isAllowedSaToteCurrency);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader[Arg.Any<string>()].Returns(string.Empty);

            const int CustId = 1;
            var expectedTotalItems = isAllowedSaToteCurrency ? 4 : 3;
            var expectedResultSports = isAllowedSaToteCurrency ? new[] { "151_41", "151_31", "152_51", "153_51" } : new[] { "151_41", "152_51", "153_51" };

            // Action
            var result = DataAccess.GetBetSetting(CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedTotalItems));
                Assert.That(result.Select(x => x.SportId).ToArray(), Is.EqualTo(expectedResultSports));
            });
        }

        [Test]
        public void GetBetSetting_GetIsAllowedSaToteCurrencyThrowsObjectDbException_ThrowDataAccessException()
        {
            // Set up for GetBetSetting
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditRB")).Returns(ObjectDb);
            Reader.Read().Returns(true, false);

            // IsAllowedSaToteCurrency method
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                "Lrf_CheckHRAllowedCurrency",
                () => DataAccess.GetBetSetting(1));
        }

        [TestCase(1, "Error")]
        [TestCase(2, "Error 2")]
        public void UpdateBetSetting_WhenCallUpdateMethodFail_ReturnResultInfo(int errorCode, string errorMsg)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);

            var racingBetSetting = GetRacingBetSetting();
            var racingBetSettingList = new Dictionary<string, RacingBetSetting> { { "151_41", racingBetSetting } };

            StubGetResultInfo(errorCode, errorMsg);

            // Action
            var result = DataAccess.UpdateBetSetting(racingBetSettingList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result["Code"], Is.EqualTo(";" + errorCode.ToString()));
                Assert.That(result["Message"], Is.EqualTo(";" + errorMsg));
            });
        }

        [TestCaseSource(nameof(GetUspNameByKey))]
        public void UpdateBetSetting_WhenCallUpdateMethod_ParametersPassedCorrectly(string key, string uspName)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            var racingBetSetting = GetRacingBetSetting();
            var racingBetSettingList = new Dictionary<string, RacingBetSetting> { { key, racingBetSetting } };

            // Action
            DataAccess.UpdateBetSetting(racingBetSettingList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, racingBetSetting.CustId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, racingBetSetting.UserId);
                ObjectDb.Received().AddInParameter("minbet", DbType.Decimal, racingBetSetting.MinBet);
                ObjectDb.Received().AddInParameter("maxbet", DbType.Decimal, racingBetSetting.MaxBet);
                ObjectDb.Received().AddInParameter("maxpermatch", DbType.Decimal, racingBetSetting.MaxPerMatch);
                ObjectDb.Received().AddInParameter("language", DbType.String, racingBetSetting.Language);
                ObjectDb.Received().AddInParameter("ip", DbType.String, racingBetSetting.Ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [TestCaseSource(nameof(GetUspNameByKey))]
        public void UpdateBetSetting_WhenObjectDbThrowsException_ThrowDataAccessException(string key, string uspName)
        {
            var racingBetSetting = GetRacingBetSetting();
            var racingBetSettingList = new Dictionary<string, RacingBetSetting> { { key, racingBetSetting } };

            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                uspName,
                () => DataAccess.UpdateBetSetting(racingBetSettingList));
        }

        private RacingBetSetting GetRacingBetSetting()
        {
            var custId = 1;
            var commonBetSetting = new CommonBetSetting
            {
                UserId = 2,
                Ip = "::1",
                Language = "LanguageTest"
            };

            return new RacingBetSetting(custId, commonBetSetting)
            {
                MinBet = 10m,
                MaxBet = 100m,
                MaxPerMatch = 30.5m,
                IsDataChange = true
            };
        }
    }
}