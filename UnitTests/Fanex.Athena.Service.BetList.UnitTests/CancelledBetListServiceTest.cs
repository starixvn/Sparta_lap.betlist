namespace Fanex.Athena.Service.BetList.UnitTests
{
    using System.Collections.Generic;
    using Core.Users.Interfaces;
    using DataAccess.BetList.Interfaces;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CancelledBetListServiceTest
    {
        private ICancelledBetListDataAccess _cancelledBetListDataAccessDataAccess;
        private IAuthorizationService _authorizationService;
        private ICancelledBetListService _betListService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _cancelledBetListDataAccessDataAccess = Substitute.For<ICancelledBetListDataAccess>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _betListService = new CancelledBetListService(_cancelledBetListDataAccessDataAccess, _authorizationService);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _cancelledBetListDataAccessDataAccess = null;
            _authorizationService = null;
            _betListService = null;
        }

        [Test]
        public void GetCancelBetCustList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CustomerCancelBetItem>
            {
                new CustomerCancelBetItem()
            };
            _cancelledBetListDataAccessDataAccess.GetCancelBetCustList(Arg.Any<BetListMetaModel>()).Returns(expectedResult);

            // Action
            var result = _betListService.GetCancelBetCustList(new BetListMetaModel());

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}