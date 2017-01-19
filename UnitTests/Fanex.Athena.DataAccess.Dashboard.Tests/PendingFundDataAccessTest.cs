namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NUnit.Framework;

    [TestFixture]
    public class PendingFundDataAccessTest
    {
        private ISimpleWidgetDataAccess _pendingFundDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _pendingFundDataAccess = new PendingFundDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 250000M };
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _pendingFundDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result.Value, Is.EqualTo(expected.Value));
        }
    }
}