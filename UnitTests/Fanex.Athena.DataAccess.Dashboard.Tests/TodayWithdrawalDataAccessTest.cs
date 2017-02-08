namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NUnit.Framework;

    [TestFixture]
    public class TodayWithdrawalDataAccessTest
    {
        private ISimpleWidgetDataAccess _todayWithDrawalDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayWithDrawalDataAccess = new TodayWithdrawalDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithDrawalDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _todayWithDrawalDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}