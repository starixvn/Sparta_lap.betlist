namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.ViewModel;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    [TestFixture]
    public partial class CustomerApiControllerTest : CultureBaseControllerHelperTest
    {
        [Test]
        public async Task MoveToGroupCom_NotAllowedCustomerComm_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = false
                });

                // Action
                var result = await controller.MoveToGroupCom(string.Empty, 0) as JsonResult;
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
        public async Task MoveToGroupCom_WhenCall_CallMoveCustomerToGroup()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string CustmerIds = "1,2";
                const int GroupId = 100;
                _productService.GetProductAccess(0, string.Empty, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });

                // Action
                await controller.MoveToGroupCom(CustmerIds, GroupId);

                // Assert
                _customerCommService.Received(1).MoveCustomersToGroupAsync(
                        Arg.Is<MoveToGroupComMetaModel>(x =>
                            x.CustomerIds == CustmerIds &&
                            x.GroupComId == GroupId &&
                            x.LicenseeId == SessionService.LicenseeSession.LicenseeId &&
                            x.UserId == SessionService.UserSession.UserId));
            }
        }

        [Test]
        public async Task MoveToGroupCom_MoveCustomerToGroupSuccess_ReturnCorrectExtendedData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string CustomerIds = "1,2";
                const int GroupId = 100;
                const decimal SampleComm = 0.05m;
                const string ExpectedComm = "5.00";
                const string GroupName = "Hello";
                const string GroupColor = "#12345";
                _productService.GetProductAccess(0, string.Empty, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });
                _customerCommService.MoveCustomersToGroupAsync(null).ReturnsForAnyArgs(Task.FromResult<IResultInfo>(new ResultInfo()));
                _groupComService.GetById(GroupId, Arg.Any<int>()).Returns(new GroupCommMetaModel
                {
                    SBComm = SampleComm,
                    NGComm = SampleComm,
                    CSComm = SampleComm,
                    GroupName = GroupName,
                    GroupColor = GroupColor
                });

                // Action
                var result = await controller.MoveToGroupCom(CustomerIds, GroupId) as JsonResult;
                var resultData = result.Data as ResultViewModel;
                var extendedData = resultData.ExtendedData;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(extendedData.GetValue("SBComm"), Is.EqualTo(ExpectedComm));
                    Assert.That(extendedData.GetValue("NGComm"), Is.EqualTo(ExpectedComm));
                    Assert.That(extendedData.GetValue("RNGComm"), Is.EqualTo(ExpectedComm));
                    Assert.That(extendedData.GetValue("GroupName"), Is.EqualTo(GroupName));
                    Assert.That(extendedData.GetValue("GroupColor"), Is.EqualTo(GroupColor));
                });
            }
        }
    }
}