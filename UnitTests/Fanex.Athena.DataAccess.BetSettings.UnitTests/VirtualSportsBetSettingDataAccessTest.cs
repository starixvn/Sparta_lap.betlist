namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility;
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
                Substitute.For<IAppSettings>(),
                CreateSessionStorage(),
                Substitute.For<ICoreLogger>(),
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitSettingSelect")).Returns(ObjectDb);
            var custId = 1;
            var productId = 5;

            // Action
            DataAccess.GetBetSetting(custId, productId);

            // Assert
            ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, custId);
            ObjectDb.Received(1).AddInParameter("productid", DbType.Int32, productId);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitSettingSelect")).Returns(ObjectDb);
            var custId = 1;
            var productId = 27;
            var expected = FakeHelper.Create<BetSetting>();

            var reader = Substitute.For<IDataReader>();
            ConfigureReaderForGetBetSettingMethod(reader, expected);

            // Action
            var result = DataAccess.GetBetSetting(custId, productId)[0];

            // Assert
            Assert.That(result.MaxBet, Is.EqualTo(expected.MaxBet));
            Assert.That(result.MinBet, Is.EqualTo(expected.MinBet));
            Assert.That(result.MaxPerMatchMax, Is.EqualTo(expected.MaxPerMatchMax));
            Assert.That(result.MaxPerMatch, Is.EqualTo(expected.MaxPerMatch));
            Assert.That(result.BetValueMax, Is.EqualTo(expected.BetValueMax));
            Assert.That(result.BetValueMin, Is.EqualTo(expected.BetValueMin));
            Assert.That(result.SportId, Is.EqualTo(expected.SportId));
            Assert.That(result.BetType, Is.EqualTo(expected.BetType));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitUpd")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var ip = "IP";
            var xmlData = "XMLDataTest";

            // Action
            DataAccess.UpdateBetSetting(custId, userId, ip, xmlData);

            // Assert
            ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, custId);
            ObjectDb.Received(1).AddInParameter("userid", DbType.Int32, userId);
            ObjectDb.Received(1).AddInParameter("ip", DbType.String, ip);
            ObjectDb.Received(1).AddInParameter("betlimit", DbType.String, xmlData);
            ObjectDb.Received(1).AddOutParameter("errcode", DbType.Int32, 4);
            ObjectDb.Received(1).AddOutParameter("errmsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnedCorrectly()
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
            var result = DataAccess.UpdateBetSetting(custId, userId, ip, xmlData);

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