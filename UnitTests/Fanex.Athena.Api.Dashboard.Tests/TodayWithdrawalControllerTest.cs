namespace Fanex.Athena.Api.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TodayWithdrawalControllerTest
    {
        private ISimpleWidgetService _todayWithdrawalService;
        private TodayWithdrawalController _todayWithdrawalController;

        [OneTimeSetUp]
        public void Setup()
        {
            _todayWithdrawalService = Substitute.For<ISimpleWidgetService>();
            _todayWithdrawalController = new TodayWithdrawalController(_todayWithdrawalService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithdrawalService = null;
            _todayWithdrawalController = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 500000M };
            var metaModel = new DashboardBaseModel();
            _todayWithdrawalService.GetData(metaModel).Returns(expected);

            // Action
            var result = _todayWithdrawalController.Index(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}