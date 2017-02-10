namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class PendingFundControllerTest
    {
        private ISimpleWidgetService _pendingFundService;
        private PendingFundController _pendingFundController;

        [OneTimeSetUp]
        public void Setup()
        {
            _pendingFundService = Substitute.For<ISimpleWidgetService>();
            _pendingFundController = new PendingFundController(_pendingFundService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundService = null;
            _pendingFundController = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel();
            _pendingFundService.GetData(metaModel).Returns(expected);

            // Action
            var result = _pendingFundController.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}