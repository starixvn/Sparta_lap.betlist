namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.ViewModel;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Session.Model;

    [TestFixture]
    public partial class CustomerApiControllerTest : CultureBaseControllerHelperTest
    {
        [Test]
        public void UpdateCustomerCom_NotAllowedCustomerComm_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = false
                });

                // Action
                var result = controller.UpdateCustomerCom(null) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(controller.Response.StatusCode, Is.EqualTo(403));
                    Assert.That(resultData.GetValue("ErrCode"), Is.EqualTo(403));
                });
            }
        }

        [Test]
        public void UpdateCustomerCom_CustomerListEmpty_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });

                // Action
                var result = controller.UpdateCustomerCom(new UpdatedCustomerComMetaModel()) as JsonResult;
                var resultData = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultData.Success, Is.False);
                    Assert.That(resultData.ErrorMessage, Is.EqualTo(ViewResources.HttpError_Default));
                });
            }
        }

        [Test]
        public void UpdateCustomerCom_CommNotValid_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });
                var metaModel = new UpdatedCustomerComMetaModel
                {
                    CustomerIds = "1",
                    SBComm = -1
                };

                // Action
                var result = controller.UpdateCustomerCom(metaModel) as JsonResult;
                var resultData = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultData.Success, Is.False);
                    Assert.That(resultData.ErrorMessage, Is.EqualTo(ViewResources.HttpError_Default));
                });
            }
        }

        [Test]
        public void UpdateCustomerCom_WhenCall_CallUpdateCustomerComm()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _productService.GetProductAccess(0, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });
                var metaModel = new UpdatedCustomerComMetaModel
                {
                    CustomerIds = "1",
                    GroupId = -1,
                    SBComm = 0.15m
                };

                // Action
                var result = controller.UpdateCustomerCom(metaModel) as JsonResult;
                var resultData = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerCommService.Received(1).UpdateCustomersCommission(metaModel, metaModel.IsAllowedCS);
                    Assert.That(resultData.ExtendedData.GetValue("SBComm"), Is.EqualTo("15.00"));
                });
            }
        }

        [Test]
        public void UpdateCustomerCom_SameOldComm_ReturnCorrectErrorMessage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _productService.GetProductAccess(0, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });
                var metaModel = new UpdatedCustomerComMetaModel
                {
                    CustomerIds = "1",
                    GroupId = 1,
                    SBComm = 0.05m
                };
                _groupComService.GetById(metaModel.GroupId).Returns(new GroupCommMetaModel
                {
                    SBComm = metaModel.SBComm
                });

                // Action
                var result = controller.UpdateCustomerCom(metaModel) as JsonResult;
                var resultData = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerCommService.DidNotReceiveWithAnyArgs().UpdateCustomersCommission(null, false);
                    Assert.That(resultData.Success, Is.False);
                    Assert.That(resultData.ErrorMessage, Is.EqualTo(ViewResources.MessageCommNoChange));
                });
            }
        }
    }
}