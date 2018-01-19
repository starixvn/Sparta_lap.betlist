namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NumberGameBetSettingDataAccessTest : BaseDataAccessHelperTest<NumberGameBetSettingDataAccess>
    {
        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditNG")).Returns(ObjectDb);
            var custId = 1;

            // Action
            DataAccess.GetBetSetting(custId);

            // Assert
            ObjectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditNG")).Returns(ObjectDb);
            var custId = 1;
            var expected = new BetSetting
            {
                MaxBet = 100.5m,
                MinBet = 10m,
                MaxPerMatchMax = 60m,
                MaxPerMatch = 40m,
                BetValueMax = 20m,
                BetValueMin = 15m,
                SportId = "161"
            };

            ConfigureReaderForGetBetSettingMethod(expected);

            // Action
            var result = DataAccess.GetBetSetting(custId)[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MaxBet, Is.EqualTo(expected.MaxBet));
                Assert.That(result.MinBet, Is.EqualTo(expected.MinBet));
                Assert.That(result.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
                Assert.That(result.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
                Assert.That(result.BetValueMax, Is.EqualTo(expected.BetValueMax));
                Assert.That(result.BetValueMin, Is.EqualTo(expected.BetValueMin));
                Assert.That(result.SportId, Is.EqualTo(expected.SportId));
            });
        }

        [Test]
        public void GetBetSetting_WhenCall_FieldCountIsZero_ReturnNull()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditNG")).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(0);

            // Action
            var result = DataAccess.GetBetSetting(0);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_MinMaxBet161")).Returns(ObjectDb);
            var param = GetNumberGameBetSetting();

            // Action
            DataAccess.UpdateBetSetting(param);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, param.CustId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, param.UserId);
                ObjectDb.Received().AddInParameter("minbetv161", DbType.Decimal, param.MinBet);
                ObjectDb.Received().AddInParameter("maxbetv161", DbType.Decimal, param.MaxBet);
                ObjectDb.Received().AddInParameter("maxpermatchv161", DbType.Decimal, param.MaxPerMatch);
                ObjectDb.Received().AddInParameter("maxperballv161", DbType.Decimal, param.MaxPerBall);
                ObjectDb.Received().AddInParameter("language", DbType.String, param.Language);
                ObjectDb.Received().AddInParameter("ip", DbType.String, param.Ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMember_MinMaxBet161")).Returns(ObjectDb);
            var param = GetNumberGameBetSetting();
            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            ObjectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            ObjectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = DataAccess.UpdateBetSetting(param);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expected.Code));
                Assert.That(result.Message, Is.EqualTo(expected.Message));
            });
        }

        private void ConfigureReaderForGetBetSettingMethod(BetSetting expected)
        {
            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);

            var sportSuffix = "v161";
            Reader["maxbet" + sportSuffix].Returns(expected.MaxBet);
            Reader["minbet" + sportSuffix].Returns(expected.MinBet);
            Reader["MaxPerMatch" + sportSuffix].Returns(expected.MaxPerMatch);
            Reader["MaxPerMatch" + sportSuffix + "_Max"].Returns(expected.MaxPerMatchMax);
            Reader["MaxPerMatch" + sportSuffix + "_Min"].Returns(expected.MaxPerMatchMin);
            Reader["MaxBet" + sportSuffix + "_Min"].Returns(expected.BetValueMax);
            Reader["MinBet" + sportSuffix + "_Max"].Returns(expected.BetValueMin);
        }

        private NumberGameBetSetting GetNumberGameBetSetting()
        {
            return new NumberGameBetSetting
            {
                CustId = 1,
                UserId = 2,
                MinBet = 10m,
                MaxBet = 100.5m,
                MaxPerMatch = 40m,
                MaxPerBall = 30m,
                Language = "LanguageTest",
                Ip = "::1"
            };
        }
    }
}