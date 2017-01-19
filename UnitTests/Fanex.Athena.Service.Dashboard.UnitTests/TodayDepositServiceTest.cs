namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TodayDepositServiceTest
    {
        private ITodayDepositDataAccess _todayDepositDataAccess;
        private ITodayDepositService _todayDepositService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _todayDepositDataAccess = Substitute.For<ITodayDepositDataAccess>();
            _todayDepositService = new TodayDepositService(_todayDepositDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _todayDepositDataAccess = null;
            _todayDepositService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 2500000M };
            var metaModel = new DashboardBaseModel();
            _todayDepositDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _todayDepositService.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
