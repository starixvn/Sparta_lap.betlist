namespace Fanex.Athena.Api.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TodayDepositControllerTest
    {
        private ISimpleWidgetService _todayDepositService;
        private TodayDepositController _todayDepositController;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayDepositService = Substitute.For<ISimpleWidgetService>();
            _todayDepositController = new TodayDepositController(_todayDepositService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositService = null;
            _todayDepositController = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            _todayDepositService.GetData(Arg.Any<DashboardBaseModel>()).Returns(expected);

            // Action
            var result = _todayDepositController.GetData(new DashboardBaseModel());

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}