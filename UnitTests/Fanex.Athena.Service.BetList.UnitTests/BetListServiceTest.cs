namespace Fanex.Athena.Service.BetList.UnitTests.Common
{
    using System.Collections.Generic;
    using Core.Configuration.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.BetList.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetListServiceTest
    {
        private IBalanceUpService _balanceUpService;
        private IBetListDataAccess _betlistDataAccess;
        private IAuthorizationService _authorizationService;
        private IBetListService _betListService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _balanceUpService = Substitute.For<IBalanceUpService>();
            _betlistDataAccess = Substitute.For<IBetListDataAccess>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _betListService = new BetListService(_balanceUpService, _betlistDataAccess, _authorizationService);

            _balanceUpService.BalanceUpInfo.Returns(new BalanceUp());
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _balanceUpService = null;
            _betlistDataAccess = null;
            _authorizationService = null;
            _betListService = null;
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new TicketResult { TotalRows = 999 };
            var licenseeId = 1;
            long transId = 1;
            _betlistDataAccess.GetMixParlayDetailTicket(transId).Returns(expectedResult);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(licenseeId, transId);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new SystemParlayData()
            {
                Outstanding = 999
            };
            _betlistDataAccess.GetSystemParlayDetail(Arg.Any<SystemParlayDetailMetaModel>()).Returns(expectedResult);

            // Action
            var result = _betListService.GetSystemParlayDetail(null);

            // Assert
            Assert.That(result.Outstanding, Is.EqualTo(expectedResult.Outstanding));
        }

        [Test]
        public void GetBetListData_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new TicketResult
            {
                TotalRows = 999
            };
            var spname = "Hello";
            _betlistDataAccess.GetBetListData(Arg.Any<BetListMetaModel>(), spname).Returns(expectedResult);

            // Action
            var result = _betListService.GetBetListData(new BetListMetaModel(), spname);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<DailyStatement>
            {
                new DailyStatement()
            };
            _betlistDataAccess.GetDailyStatementData(Arg.Any<BetListMetaModel>(), false).Returns(expectedResult);

            // Action
            var result = _betListService.GetDailyStatementData(new BetListMetaModel(), false);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}