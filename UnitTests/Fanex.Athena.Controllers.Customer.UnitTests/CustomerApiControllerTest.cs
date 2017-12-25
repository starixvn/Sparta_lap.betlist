namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Customer;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.ViewModel;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Customer.Interfaces;
    using UnitTest.Common.Extensions;

    [TestFixture]
    public partial class CustomerApiControllerTest : BaseControllerHelperTest
    {
        private ICustomerCommService _customerCommService;
        private ICustomerListService _customerListService;
        private IGroupComService _groupComService;
        private ILicenseeDataAccess _licenseeDataAccess;
        private IProductService _productService;

        [SetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            SetUpDefaultFakeData();
            _customerCommService = Substitute.For<ICustomerCommService>();
            _customerListService = Substitute.For<ICustomerListService>();
            _groupComService = Substitute.For<IGroupComService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _productService = Substitute.For<IProductService>();
        }

        [TearDown]
        public void TearDown()
        {
            AppSettings.ClearReceivedCalls();
            _customerListService.ClearReceivedCalls();
            _groupComService.ClearReceivedCalls();
        }

        [Test]
        public void UpdateCustStatus_LicenseeNotAllowStatusFunction_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.GetValue("LicenseeOfferStatusFunction").Returns("0");

                // Action
                var result = controller.UpdateCustStatus(0, false, false) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.IsSuccessful, Is.False);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionNoAvailableMsg));
                });
            }
        }

        [Test]
        public void UpdateCustStatus_LicenseeAllowStatusFunction_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int CustId = 200;
                bool isClosed = true;
                bool isSuspended = true;
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                AppSettings.LicenseeOfferStatusFunction.Returns(new[] { licenseeId });

                // Action
                controller.UpdateCustStatus(CustId, isClosed, isSuspended);

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).UpdateCustClosed(
                        Arg.Is<CustomerStatusMetaModel>(x =>
                        x.LicenseeId == licenseeId &&
                        x.LicenseeName == SessionService.LicenseeSession.LicenseeName &&
                        x.CustId == CustId &&
                        x.IsOn == isClosed &&
                        x.ModifyById == SessionService.UserSession.UserId &&
                        x.ModifyByUsername == SessionService.UserSession.UserName &&
                        x.Ip == controller.Request.UserHostAddress),
                        SessionService.UserSession.Language);

                    _customerListService.Received(1).UpdateCustSuspend(
                        Arg.Is<CustomerStatusMetaModel>(x =>
                        x.LicenseeId == licenseeId &&
                        x.LicenseeName == SessionService.LicenseeSession.LicenseeName &&
                        x.CustId == CustId &&
                        x.IsOn == isSuspended &&
                        x.ModifyById == SessionService.UserSession.UserId &&
                        x.ModifyByUsername == SessionService.UserSession.UserName &&
                        x.Ip == controller.Request.UserHostAddress),
                        SessionService.UserSession.Language);
                });
            }
        }

        [Test]
        public void UpdateSpecialLevel_FakeLicensee_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var fakeLicenseeList = new[] { SessionService.LicenseeSession.LicenseeId };
                AppSettings.FakeLicenseeList.Returns(fakeLicenseeList);

                // Action
                var result = controller.UpdateSpecialLevel(0, 0) as JsonResult;
                var resultInfo = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.DidNotReceiveWithAnyArgs().UpdateSpecialLevel(0, 0, 0, 0, string.Empty, string.Empty);
                    Assert.That(resultInfo.Success, Is.False);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateSpecialLevel_RealLicensee_CallServiceToUpdate()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int CustId = 100;
                const int Level = 200;

                // Action
                controller.UpdateSpecialLevel(CustId, Level);

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).UpdateSpecialLevel(
                        SessionService.LicenseeSession.LicenseeId,
                        SessionService.UserSession.UserId,
                        CustId,
                        Level,
                        Arg.Any<string>(),
                        SessionService.UserSession.Language);
                });
            }
        }

        [Test]
        public void UpdateOddsGroup_FakeLicensee_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var fakeLicenseeList = new[] { SessionService.LicenseeSession.LicenseeId };
                AppSettings.FakeLicenseeList.Returns(fakeLicenseeList);

                // Action
                var result = controller.UpdateOddsGroup(0, string.Empty) as JsonResult;
                var resultInfo = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.DidNotReceiveWithAnyArgs().UpdateOddsGroup(Arg.Any<CustomerOddsGroupMetaModel>());
                    Assert.That(resultInfo.Success, Is.False);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateOddsGroup_RealLicensee_CallServiceToUpdate()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int CustId = 100;
                const string OddsGroup = "abc";

                // Action
                controller.UpdateOddsGroup(CustId, OddsGroup);

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).UpdateOddsGroup(
                        Arg.Is<CustomerOddsGroupMetaModel>(x =>
                        x.LicenseeId == SessionService.LicenseeSession.LicenseeId &&
                        x.CustId == CustId &&
                        x.Ip == controller.Request.UserHostAddress &&
                        x.Language == SessionService.UserSession.Language &&
                        x.OddsGroup == OddsGroup &&
                        x.UserId == SessionService.UserSession.UserId));
                });
            }
        }

        [Test]
        public void RemoveFromGroupCom_NotAllowedCustomerComm_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = false
                });

                // Action
                var result = controller.RemoveFromGroupCom(string.Empty, false) as JsonResult;
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
        public void RemoveFromGroupCom_WhenCall_CallRemoveCustomerComm()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string CustmerIds = "1,2";
                bool isRemovePlayerComm = true;
                _productService.GetProductAccess(0, string.Empty, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));
                SessionService.LicenseeSession.Returns(new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                });

                // Action
                var result = controller.RemoveFromGroupCom(CustmerIds, isRemovePlayerComm) as JsonResult;
                var resultData = result.Data as ResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerCommService.Received(1).RemoveCustomersFromGroup(
                        Arg.Is<RemoveFromGroupComMetaModel>(x =>
                        x.CustomerIds == CustmerIds && x.IsRemovePlayercomm == isRemovePlayerComm),
                        false);
                    Assert.That(resultData.ExtendedData.GetValue("GroupName"), Is.EqualTo("None"));
                    Assert.That(resultData.ExtendedData.GetValue("GroupColor"), Is.EqualTo("#ffffff"));
                });
            }
        }

        [Test]
        public void SearchCustomer_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Query = " test   ";
                const string AgentListXml = "Agent List XML";
                var expectedCustomerNames = new[] { "test1", "test2" };
                SessionService.GenerateAgentListXml().Returns(AgentListXml);
                _customerListService.SearchCustomers(0, 0, string.Empty, string.Empty)
                    .ReturnsForAnyArgs(expectedCustomerNames);

                // Action
                var result = controller.SearchCustomer(Query) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).SearchCustomers(
                        SessionService.LicenseeSession.LicenseeId,
                        SessionService.UserSession.UserId,
                        Query.Trim(),
                        AgentListXml);
                    Assert.That(resultData.GetValue("query"), Is.EqualTo(Query.Trim()));
                    Assert.That(resultData.GetValue("suggestions"), Is.EqualTo(expectedCustomerNames));
                });
            }
        }

        private CustomerApiController CreateController()
        {
            return new CustomerApiController(
               LangService,
               SessionService,
               AppSettings,
               _customerCommService,
               _customerListService,
               _groupComService,
               _licenseeDataAccess,
               _productService,
               ClientViewHelper)
            {
                ControllerContext = ControllerContext
            };
        }
    }
}