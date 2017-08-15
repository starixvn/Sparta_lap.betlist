namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Dashboard;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class CustomerBalanceControllerTest
    {
        private ISimpleWidgetService _customerBalanceService;

        [OneTimeSetUp]
        public void Setup()
        {
            _customerBalanceService = Substitute.For<ISimpleWidgetService>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _customerBalanceService = null;
        }

        [TestCase("CLMV")]
        public void GetData_InvalidBaseCurrency_ReturnZeroAndDoNotCallService(string baseCurrency)
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = baseCurrency
            };

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.Zero);
                _customerBalanceService.DidNotReceive().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", CurrencyId = 1, SelectedWidgetString = "CustomerBalance" };
            _customerBalanceService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                controller.GetData(metaModel);

                // Assert
                Assert.That(metaModel.LicenseeId, Is.EqualTo(1));
                _customerBalanceService.Received().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };

            _customerBalanceService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private CustomerBalanceController CreateController()
        {
            var controller = new CustomerBalanceController(_customerBalanceService);
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            controller.ActionContext = mockActionContext;

            return controller;
        }
    }
}