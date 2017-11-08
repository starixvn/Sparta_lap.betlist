namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayDepositControllerTest
    {
        private ISingleValueDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.For<ISingleValueDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
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
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };
            _dataAccess.GetTodayDeposit(metaModel).Returns(expected);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetData(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private TodayDepositController CreateController()
        {
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            return new TodayDepositController(_dataAccess)
            {
                ActionContext = mockActionContext
            };
        }
    }
}