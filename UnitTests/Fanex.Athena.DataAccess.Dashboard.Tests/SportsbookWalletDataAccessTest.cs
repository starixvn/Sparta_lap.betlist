namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NUnit.Framework;

    public class SportsbookWalletDataAccessTest
    {
        private ISimpleWidgetDataAccess _sportsbookWalletDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sportsbookWalletDataAccess = new CustomerBalanceDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sportsbookWalletDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 1570000M;
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _sportsbookWalletDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
