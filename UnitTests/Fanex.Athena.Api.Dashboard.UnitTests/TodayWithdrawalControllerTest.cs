namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayWithdrawalControllerTest
    {
        private ISimpleWidgetDataAccess _todayWithdrawalService;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayWithdrawalService = Substitute.For<ISimpleWidgetDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithdrawalService = null;
        }

        [TestCase("CLMV")]
        public void GetData_InvalidBaseCurrency_ReturnZero(string baseCurrency)
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
                _todayWithdrawalService.DidNotReceive().GetData(metaModel);
            }
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _todayWithdrawalService.GetData(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private TodayWithdrawalController CreateController()
        {
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            return new TodayWithdrawalController(_todayWithdrawalService)
            {
                ActionContext = mockActionContext
            };
        }
    }
}