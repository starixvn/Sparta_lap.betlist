namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public partial class CustomerApiControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void UpdateBAStatus_FakeLicensee_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                bool isAllowBa = true;
                var productAccess = new ProductAccess(new List<Product>());
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(productAccess);

                // Action
                var result = controller.UpdateBAStatus(0, isAllowBa) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.DidNotReceiveWithAnyArgs().UpdateCustomerBAStatus(0, 0, 0, false, null);
                    Assert.That(resultData.GetValue("IsSuccessful"), Is.False);
                    Assert.That(resultData.GetValue("Message"), Is.EqualTo(ViewResources.NotAllowBA));
                    Assert.That(resultData.GetValue("IsBA"), Is.EqualTo(isAllowBa));
                    Assert.That(resultData.GetValue("BAStatus"), Is.EqualTo("On"));
                });
            }
        }

        [Test]
        public void UpdateBAStatus_NotSpecialBaAndNotAllowUpdateBa_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResultMessage = "konnichiwa";
                bool isAllowBa = true;
                SessionService.IsAllowedSpecialBA(Arg.Any<UserSession>()).Returns(false);
                var productAccess = new ProductAccess(new List<Product>
                {
                    new Product { ProductCode = Product.BA }
                });
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(productAccess);
                bool isAllowUpdateBa = false;
                _licenseeDataAccess.IsAllowUpdateMemberBAStatus(Arg.Any<Request>(), 0, out isAllowUpdateBa)
                    .ReturnsForAnyArgs(new ResultInfo
                    {
                        Message = ResultMessage
                    });

                // Action
                var result = controller.UpdateBAStatus(0, isAllowBa) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.DidNotReceiveWithAnyArgs().UpdateCustomerBAStatus(0, 0, 0, false, null);
                    Assert.That(resultData.GetValue("IsSuccessful"), Is.False);
                    Assert.That(resultData.GetValue("Message"), Is.EqualTo(ResultMessage));
                    Assert.That(resultData.GetValue("IsBA"), Is.EqualTo(isAllowBa));
                    Assert.That(resultData.GetValue("BAStatus"), Is.EqualTo("On"));
                });
            }
        }

        [Test]
        public void UpdateBAStatus_WhenCall_CallUpdateBaStatus()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string OddsGroup = "Z";
                const int CustId = 100;
                bool isAllowBa = true;
                SessionService.IsAllowedSpecialBA(Arg.Any<UserSession>()).Returns(true);
                var productAccess = new ProductAccess(new List<Product>
                {
                    new Product { ProductCode = Product.BA }
                });
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(productAccess);
                _customerListService.UpdateCustomerBAStatus(0, 0, 0, false, null)
                    .ReturnsForAnyArgs(new ResultInfo
                    {
                        ExtendedData = OddsGroup
                    });

                // Action
                var result = controller.UpdateBAStatus(CustId, isAllowBa) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).UpdateCustomerBAStatus(
                        SessionService.LicenseeSession.LicenseeId,
                        CustId,
                        SessionService.UserSession.UserId,
                        isAllowBa,
                        Arg.Any<Request>());
                    Assert.That(resultData.GetValue("IsSuccessful"), Is.True);
                    Assert.That(resultData.GetValue("OddsGroup"), Is.EqualTo(OddsGroup));
                });
            }
        }
    }
}