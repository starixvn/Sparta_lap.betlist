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
            var expected = new SimpleWidgetViewModel { Value = 2500000M };
            var metaModel = new DashboardBaseModel();
            _todayDepositService.GetData(metaModel).Returns(expected);

            // Action
            var result = _todayDepositController.Index(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}