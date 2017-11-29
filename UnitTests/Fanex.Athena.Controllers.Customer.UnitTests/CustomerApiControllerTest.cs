namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Customer;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Customer.Interfaces;

    [TestFixture]
    public partial class CustomerApiControllerTest : BaseControllerHelperTest
    {
        private ICustomerCommService _customerCommService;
        private ICustomerListService _customerListService;
        private IGroupComService _groupComService;
        private ILicenseeDataAccess _licenseeDataAccess;
        private IProductService _productService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _customerCommService = Substitute.For<ICustomerCommService>();
            _customerListService = Substitute.For<ICustomerListService>();
            _groupComService = Substitute.For<IGroupComService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _productService = Substitute.For<IProductService>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _customerCommService = null;
            _customerListService = null;
            _licenseeDataAccess = null;
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeData();
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
                AppSettings.GetValue("LicenseeOfferStatusFunction").Returns(DefaultLicenseeSession.LicenseeId.ToString());

                // Action
                controller.UpdateCustStatus(CustId, isClosed, isSuspended);

                // Assert
                Assert.Multiple(() =>
                {
                    _customerListService.Received(1).UpdateCustClosed(
                        Arg.Is<CustomerStatusMetaModel>(x =>
                        x.LicenseeId == SessionService.LicenseeSession.LicenseeId &&
                        x.LicenseeName == SessionService.LicenseeSession.LicenseeName &&
                        x.CustId == CustId &&
                        x.IsOn == isClosed &&
                        x.ModifyById == SessionService.UserSession.UserId &&
                        x.ModifyByUsername == SessionService.UserSession.UserName &&
                        x.Ip == controller.Request.UserHostAddress),
                        SessionService.UserSession.Language);

                    _customerListService.Received(1).UpdateCustSuspend(
                        Arg.Is<CustomerStatusMetaModel>(x =>
                        x.LicenseeId == SessionService.LicenseeSession.LicenseeId &&
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

        private CustomerApiController CreateController()
        {
            return new CustomerApiController(
               LangService,
               SessionService,
               AppSettings,
               FileSystem,
               _customerCommService,
               _customerListService,
               _groupComService,
               _licenseeDataAccess,
               _productService)
            {
                ControllerContext = ControllerContext
            };
        }
    }
}