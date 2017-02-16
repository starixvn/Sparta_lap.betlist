namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayWithdrawalServiceTest
    {
        private ISimpleWidgetDataAccess _todayWithdrawalDataAccess;
        private ISimpleWidgetService _todayWithdrawalService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayWithdrawalDataAccess = Substitute.For<ISimpleWidgetDataAccess>();
            _todayWithdrawalService = new TodayWithdrawalService(_todayWithdrawalDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayWithdrawalDataAccess = null;
            _todayWithdrawalService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var metaModel = new DashboardBaseModel();
            _todayWithdrawalDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _todayWithdrawalService.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}