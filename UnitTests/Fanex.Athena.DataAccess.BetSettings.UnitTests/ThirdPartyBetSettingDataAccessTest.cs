namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ThirdPartyBetSettingDataAccessTest : BaseDataAccessHelperTest<ThirdPartyBetSettingDataAccess>
    {
        [Test]
        public void GetBetSetting_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductSelCustStatus")).Returns(ObjectDb);
            var custId = 1;
            var productId = 2;

            // Action
            DataAccess.GetBetSetting(custId, productId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("CustID", custId);
            ObjectDb.Received(1).SetParameterValue("ProductId", productId);
            ObjectDb.Received(1).SetParameterValue("IsEnabled", null);
            ObjectDb.Received(1).SetParameterValue("IsLineEnabled", null);
            ObjectDb.Received(1).SetParameterValue("Currency", null);
        }

        [Test]
        public void GetBetSetting_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductSelCustStatus")).Returns(ObjectDb);
            var custId = 1;
            var productId = 2;
            var expected = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = false,
                CurrencyId = 1
            };

            ObjectDb.GetParameterValue("IsEnabled").Returns(expected.IsEnabled);
            ObjectDb.GetParameterValue("IsLineEnabled").Returns(expected.IsLineEnabled);
            ObjectDb.GetParameterValue("Currency").Returns(expected.CurrencyId);

            // Action
            var result = DataAccess.GetBetSetting(custId, productId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(expected.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(expected.IsLineEnabled));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
        }

        [Test]
        public void UpdateStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductUpdCustStatus")).Returns(ObjectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "IP";
            var isEnabled = true;

            // Action
            DataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", custId);
            ObjectDb.Received(1).SetParameterValue("userid", userId);
            ObjectDb.Received(1).SetParameterValue("productid", productId);
            ObjectDb.Received(1).SetParameterValue("isenabled", isEnabled);
            ObjectDb.Received(1).SetParameterValue("ip", ip);
            ObjectDb.Received(1).SetParameterValue("language", language);
            ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_3rdProductUpdCustStatus")).Returns(ObjectDb);
            var productId = 1;
            var userId = 2;
            var custId = 3;
            var language = "Language Test";
            var ip = "IP";
            var isEnabled = true;

            var expected = new ResultInfo
            {
                Code = 0,
                Message = "Success"
            };
            ObjectDb.GetParameterValue("ErrMsg").Returns(expected.Message);

            // Action
            var result = DataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }
    }
}