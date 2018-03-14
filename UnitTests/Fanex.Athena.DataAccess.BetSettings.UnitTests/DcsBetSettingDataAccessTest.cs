namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Constant.DataDomain;
    using Core.UnitTests.TestHelpers;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DcsBetSettingDataAccessTest : BaseDataAccessHelperTest<DcsBetSettingDataAccess>
    {
        [Test]
        public void GetStatus_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(ObjectDb);
            const int CustId = 1;
            const int ProductId = 2;

            // Action
            DataAccess.GetStatus(CustId, ProductId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("CustID", DbType.Int32, CustId);
                ObjectDb.Received().AddInParameter("ProductId", DbType.Int32, ProductId);
            });
        }

        [TestCase(true, 0, true)]
        [TestCase(true, 1, false)]
        [TestCase(false, 0, null)]
        [TestCase(false, 1, null)]
        public void GetStatus_WhenCall_DataReturnCorrectly(bool hasData, int? isDisabled, bool? expectedResult)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(ObjectDb);

            Reader.Read().Returns(hasData);
            Reader["IsDisabled"].Returns(isDisabled);

            const int CustId = 1;
            const int ProductId = 2;

            // Action
            var result = DataAccess.GetStatus(CustId, ProductId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateStatus_WhenCall_ParameterPassedCorrectly()
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
            DataAccess.UpdateStatus(userId, custId, ip, status, productId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, custId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, userId);
                ObjectDb.Received().AddInParameter("IsDisabled", DbType.String, ExpectedIsDisabledParam);
                ObjectDb.Received().AddInParameter("ip", DbType.String, ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 8);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
            });
        }

        [Test]
        public void UpdateStatus_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Acc_Dep_CustSetting_DisableStatUpd")).Returns(ObjectDb);
            ObjectDb.GetParameterValue("ErrCode").Returns(1);
            ObjectDb.GetParameterValue("ErrMsg").Returns("abc");

            // Action
            var result = DataAccess.UpdateStatus(0, 0, string.Empty, false, ProductTypes.AllBet);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo("abc"));
            });
        }

        [Test]
        public void UpdateBetSetting_WhenCall_ParameterPassedCorrectly()
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
            DataAccess.UpdateBetSetting(CustId, UserId, Ip, XmlData);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("ip", DbType.String, Ip);
                ObjectDb.Received().AddInParameter("betlimit", DbType.String, XmlData);
                ObjectDb.Received().AddOutParameter("errcode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("errmsg", DbType.String, 8000);
            });
        }

        [Test]
        public void UpdateBetSetting_WhenCall_DataReturnCorrectly()
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
            var result = DataAccess.UpdateBetSetting(CustId, UserId, Ip, XmlData);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo("Error"));
            });
        }
    }
}