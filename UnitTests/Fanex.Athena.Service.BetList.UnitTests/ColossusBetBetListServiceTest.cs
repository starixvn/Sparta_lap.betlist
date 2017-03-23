namespace Fanex.Athena.Service.BetList.UnitTests
{
    using DataAccess.BetList.Interfaces;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ColossusBetBetListServiceTest
    {
        private IColossusBetBetListDataAccess _colossusBetBetListDataAccess;
        private ColossusBetBetListService _colossusBetBetListService;

        [SetUp]
        public void SetUp()
        {
            _colossusBetBetListDataAccess = Substitute.For<IColossusBetBetListDataAccess>();
            _colossusBetBetListService = new ColossusBetBetListService(_colossusBetBetListDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _colossusBetBetListDataAccess = null;
            _colossusBetBetListService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new ColossusBetDetailViewModel();
            _colossusBetBetListDataAccess.GetData(Arg.Any<ColossusBetDetailMetaModel>()).Returns(expectedResult);

            // Action
            var result = _colossusBetBetListService.GetData(new ColossusBetDetailMetaModel());

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetSiteId_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int CustId = 1;
            var expectedResult = 100;
            _colossusBetBetListDataAccess.GetSiteId(CustId).Returns(expectedResult);

            // Action
            var result = _colossusBetBetListService.GetSiteId(CustId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}