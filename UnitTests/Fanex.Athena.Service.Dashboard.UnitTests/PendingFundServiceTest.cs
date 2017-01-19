namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PendingFundServiceTest
    {
        private ISimpleWidgetDataAccess _pendingFundDataAccess;
        private ISimpleWidgetService _pendingFundService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _pendingFundDataAccess = Substitute.For<ISimpleWidgetDataAccess>();
            _pendingFundService = new PendingFundService(_pendingFundDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundDataAccess = null;
            _pendingFundService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 250000M };
            var metaModel = new DashboardBaseModel();
            _pendingFundDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _pendingFundService.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}