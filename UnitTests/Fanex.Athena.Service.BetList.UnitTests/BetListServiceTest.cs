namespace Fanex.Athena.Service.BetList.UnitTests
{
    using System.Collections.Generic;
    using Core.Users.Interfaces;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.UnitTest.Common.Extensions;
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
        private IBetListDataAccess _betlistDataAccess;
        private IAuthorizationService _authorizationService;
        private IBetListService _betListService;

        [OneTimeSetUp]
        public void SetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _betlistDataAccess = Substitute.For<IBetListDataAccess>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _betListService = new BetListService(balanceUpDataAccess, _betlistDataAccess, _authorizationService);

            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
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
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _betlistDataAccess.GetMixParlayDetailTicket(transId).Returns(expectedResult);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(licenseeId, transId);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetMixParlayDetailTicket_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new TicketResult();
            var licenseeId = 1;
            long transId = 1;
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(licenseeId, transId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new SystemParlayData()
            {
                Outstanding = 999
            };
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _betlistDataAccess.GetBetListData(Arg.Any<BetListMetaModel>(), spname).Returns(expectedResult);

            // Action
            var result = _betListService.GetBetListData(new BetListMetaModel(), spname);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetBetListData_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new TicketResult();
            var spname = "Hello";
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _betListService.GetBetListData(new BetListMetaModel(), spname);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<DailyStatement>
            {
                new DailyStatement()
            };
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _betlistDataAccess.GetDailyStatementData(Arg.Any<BetListMetaModel>(), false).Returns(expectedResult);

            // Action
            var result = _betListService.GetDailyStatementData(new BetListMetaModel(), false);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }

        [Test]
        public void GetDailyStatementData_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _betListService.GetDailyStatementData(new BetListMetaModel(), false);

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}