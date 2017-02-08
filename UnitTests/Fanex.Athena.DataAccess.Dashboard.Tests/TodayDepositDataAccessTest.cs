using Fanex.Athena.DataAccess.Dashboard.Interfaces;
using Fanex.Athena.Model.Dashboard.MetaModel;
using Fanex.Athena.Model.Dashboard.ViewModel;
using NUnit.Framework;

namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    [TestFixture]
    public class TodayDepositDataAccessTest
    {
        private ISimpleWidgetDataAccess _todayDepositDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayDepositDataAccess = new TodayDepositDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 2500000M;
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _todayDepositDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
