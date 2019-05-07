namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using Core.UnitTests.TestHelpers;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class DcsBetSettingDataAccessTest : BaseDataAccessHelperTest<DcsBetSettingDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<DcsBetSettingDataAccess>(
                Substitute.For<ICoreLogger>(),
                Substitute.For<ICacheRefAdapter>());
        }

        [Test]
        public async Task GetStatusAsync_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(ObjectDb);
            const int CustId = 1;
            const int ProductId = 2;

            // Action
            await DataAccess.GetStatusAsync(CustId, ProductId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("CustID", CustId);
                ObjectDb.Received(1).SetParameterValue("ProductId", ProductId);
            });
        }

        [TestCase(true, 0, true)]
        [TestCase(true, 1, false)]
        [TestCase(false, 0, null)]
        [TestCase(false, 1, null)]
        public async Task GetStatusAsync_WhenCall_DataReturnCorrectly(bool hasData, int? isDisabled, bool? expectedResult)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(ObjectDb);

            Reader.Read().Returns(hasData);
            Reader["IsDisabled"].Returns(isDisabled);

            const int CustId = 1;
            const int ProductId = 2;

            // Action
            var result = await DataAccess.GetStatusAsync(CustId, ProductId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            const string ExpectedIsDisabledParam = @"<ROOT><ProductStatus SportType=""211"" BetType=""2401"" IsDisabled=""0"" IsDisabledDownline=""0"" /></ROOT>";
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_DisableStatUpd")).Returns(ObjectDb);
            var custId = 1;
            var userId = 2;
            var productId = ProductTypes.AllBet;
            var status = true;
            var ip = "local";
            ObjectDb.GetParameterValue("ErrCode").Returns(1);

            // Action
            await DataAccess.UpdateStatusAsync(userId, custId, ip, status, productId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", custId);
                ObjectDb.Received(1).SetParameterValue("userid", userId);
                ObjectDb.Received(1).SetParameterValue("IsDisabled", ExpectedIsDisabledParam);
                ObjectDb.Received(1).SetParameterValue("ip", ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_DisableStatUpd")).Returns(ObjectDb);
            ObjectDb.GetParameterValue("ErrCode").Returns(1);
            ObjectDb.GetParameterValue("ErrMsg").Returns("abc");

            // Action
            var result = await DataAccess.UpdateStatusAsync(0, 0, string.Empty, false, ProductTypes.AllBet);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo("abc"));
            });
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            const int CustId = 1;
            const int UserId = 2;
            const string Ip = "IP";
            const string XmlData = "XmlData";

            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitUpd")).Returns(ObjectDb);
            ObjectDb.GetParameterValue("ErrCode").Returns(1);
            ObjectDb.GetParameterValue("ErrMsg").Returns(string.Empty);

            // Action
            await DataAccess.UpdateBetSettingAsync(CustId, UserId, Ip, XmlData);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", CustId);
                ObjectDb.Received(1).SetParameterValue("userid", UserId);
                ObjectDb.Received(1).SetParameterValue("ip", Ip);
                ObjectDb.Received(1).SetParameterValue("betlimit", XmlData);
                ObjectDb.Received(1).SetParameterValue("errcode", null);
                ObjectDb.Received(1).SetParameterValue("errmsg", null);
            });
        }

        [Test]
        public async Task UpdateBetSettingAsync_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            const int CustId = 1;
            const int UserId = 2;
            const string Ip = "IP";
            const string XmlData = "XmlData";

            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_BetLimitUpd")).Returns(ObjectDb);
            ObjectDb.GetParameterValue("ErrCode").Returns(1);
            ObjectDb.GetParameterValue("ErrMsg").Returns("<Root Msg='Error'></Root>");

            // Action
            var result = await DataAccess.UpdateBetSettingAsync(CustId, UserId, Ip, XmlData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo("Error"));
            });
        }
    }
}