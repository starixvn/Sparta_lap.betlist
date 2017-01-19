namespace Fanex.Athena.Api.Dashboard.Tests
{
    using Dashboard;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class SportsbookWalletControllerTest
    {
        private ISimpleWidgetService _sportsbookWalletService;
        private SportsbookWalletController _sportsbookWalletController;

        [OneTimeSetUp]
        public void Setup()
        {
            _sportsbookWalletService = Substitute.For<ISimpleWidgetService>();
            _sportsbookWalletController = new SportsbookWalletController(_sportsbookWalletService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sportsbookWalletService = null;
            _sportsbookWalletController = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 1570000M };
            var metaModel = new DashboardBaseModel();
            _sportsbookWalletService.GetData(metaModel).Returns(expected);

            // Action
            var result = _sportsbookWalletController.Index(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}