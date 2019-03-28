namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class VirtualSportsBetSettingDataAccessTest : BaseDataAccessHelperTest<DcsBetSettingDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<DcsBetSettingDataAccess>(
                Substitute.For<ICoreLogger>(),
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetBetSettingAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitSettingSelect")).Returns(ObjectDb);
            var custId = 1;
            var productId = 5;

            // Action
            await DataAccess.GetBetSettingAsync(custId, productId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", custId);
            ObjectDb.Received(1).SetParameterValue("productid", productId);
        }

        [Test]
        public async Task GetBetSettingAsync_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitSettingSelect")).Returns(ObjectDb);
            var custId = 1;
            var productId = 27;
            var expected = FakeHelper.Create<BetSetting>();

            var reader = Substitute.For<IDataReader>();
            ConfigureReaderForGetBetSettingMethod(reader, expected);

            // Action
            var result = await DataAccess.GetBetSettingAsync(custId, productId);
            var firstItem = result[0];

            // Assert
            Assert.That(firstItem.MaxBet, Is.EqualTo(expected.MaxBet));
            Assert.That(firstItem.MinBet, Is.EqualTo(expected.MinBet));
            Assert.That(firstItem.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
            Assert.That(firstItem.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
            Assert.That(firstItem.BetValueMax, Is.EqualTo(expected.BetValueMax));
            Assert.That(firstItem.BetValueMin, Is.EqualTo(expected.BetValueMin));
            Assert.That(firstItem.SportId, Is.EqualTo(expected.SportId));
            Assert.That(firstItem.BetType, Is.EqualTo(expected.BetType));
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitUpd")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var ip = "IP";
            var xmlData = "XMLDataTest";

            // Action
            await DataAccess.UpdateBetSettingAsync(custId, userId, ip, xmlData);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", custId);
            ObjectDb.Received(1).SetParameterValue("userid", userId);
            ObjectDb.Received(1).SetParameterValue("ip", ip);
            ObjectDb.Received(1).SetParameterValue("betlimit", xmlData);
            ObjectDb.Received(1).SetParameterValue("errcode", null);
            ObjectDb.Received(1).SetParameterValue("errmsg", null);
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitUpd")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var ip = "IP";
            var xmlData = "XMLDataTest";

            var expected = new ResultInfo
            {
                Code = 100,
                Message = "Success"
            };
            ObjectDb.GetParameterValue("ErrCode").Returns(expected.Code);
            ObjectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = await DataAccess.UpdateBetSettingAsync(custId, userId, ip, xmlData);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        private void ConfigureReaderForGetBetSettingMethod(IDataReader reader, BetSetting expected)
        {
            ObjectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            reader["sporttype"].Returns(expected.SportId);
            reader["minbet"].Returns(expected.MinBet);
            reader["minbetmin"].Returns(expected.BetValueMin);
            reader["maxbet"].Returns(expected.MaxBet);
            reader["maxbetmax"].Returns(expected.BetValueMax);
            reader["maxpermatch"].Returns(expected.MaxPerMatch);
            reader["maxpermatchmax"].Returns(expected.MaxPerMatchMax);
            reader["bettype"].Returns(expected.BetType);
        }
    }
}