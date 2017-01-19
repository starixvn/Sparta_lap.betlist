namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SportsbookWalletServiceTest
    {
        private ISportsbookWalletDataAccess _sportsbookWalletDataAccess;
        private ISportsbookWalletService _sportsbookWalletService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sportsbookWalletDataAccess = Substitute.For<ISportsbookWalletDataAccess>();
            _sportsbookWalletService = new SportsbookWalletService(_sportsbookWalletDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sportsbookWalletDataAccess = null;
            _sportsbookWalletService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new SimpleWidgetViewModel { Value = 1570000M };
            var metaModel = new DashboardBaseModel();
            _sportsbookWalletDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _sportsbookWalletService.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}