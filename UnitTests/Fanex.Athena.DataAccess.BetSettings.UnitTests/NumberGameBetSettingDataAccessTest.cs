namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Constant.DataDomain;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Utility;

    [TestFixture]
    public class NumberGameBetSettingDataAccessTest : BaseDataAccessHelperTest<NumberGameBetSettingDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            var betSettingReader = Substitute.For<IBetSettingReader>();
            betSettingReader.ReadForSport(Arg.Any<IDataReader>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(x => new BetSetting
                {
                    SportId = x[1].ToString()
                });
            DataAccess = Substitute.ForPartsOf<NumberGameBetSettingDataAccess>(betSettingReader);
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditNG")).Returns(ObjectDb);
            var custId = 1;

            // Action
            DataAccess.GetBetSetting(custId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("CustID", custId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelEditNG")).Returns(ObjectDb);
            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);

            // Action
            var result = DataAccess.GetBetSetting(0)[0];

            // Assert
            Assert.That(result.SportId, Is.EqualTo(((int)SportType.NumberGame).ToString()));
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
                ObjectDb.Received(1).SetParameterValue("custid", param.CustId);
                ObjectDb.Received(1).SetParameterValue("userid", param.UserId);
                ObjectDb.Received(1).SetParameterValue("minbetv161", param.MinBet);
                ObjectDb.Received(1).SetParameterValue("maxbetv161", param.MaxBet);
                ObjectDb.Received(1).SetParameterValue("maxpermatchv161", param.MaxPerMatch);
                ObjectDb.Received(1).SetParameterValue("maxperballv161", param.MaxPerBall);
                ObjectDb.Received(1).SetParameterValue("language", param.Language);
                ObjectDb.Received(1).SetParameterValue("ip", param.Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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
                Ip = "IP"
            };
        }
    }
}