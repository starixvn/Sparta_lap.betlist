namespace Fanex.Athena.Service.BetList.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.BetList.Core.Entities;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using StatelessCore.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BetListServiceTest
    {
        private IBetListDataAccess _betlistDataAccess;
        private IAuthorizationService _authorizationService;
        private IBetListService _betListService;
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

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
            var expectedResult = FakeHelper.Create<TicketResult>();
            const int LicenseeId = 1;
            const long TransId = 1;
            const int CustId = 2;
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _betlistDataAccess.GetMixParlayDetailTicket(_currencyList, TransId, CustId).Returns(expectedResult);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(_currencyList, LicenseeId, TransId, CustId);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetMixParlayDetailTicket_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new TicketResult();
            const int LicenseeId = 1;
            const long TransId = 1;
            const int CustId = 2;
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(_currencyList, LicenseeId, TransId, CustId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new SystemParlayData { Outstanding = 999 };
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
            var expectedResult = FakeHelper.Create<TicketResult>();
            var spname = FakeHelper.Create<string>();
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _betlistDataAccess.GetBetListData(Arg.Any<BetListMetaModel>(), _currencyList, spname).Returns(expectedResult);

            // Action
            var result = _betListService.GetBetListData(new BetListMetaModel(), _currencyList, spname);

            // Assert
            Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
        }

        [Test]
        public void GetBetListData_HaveNoPermission_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new TicketResult();
            var spname = FakeHelper.Create<string>();
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _betListService.GetBetListData(new BetListMetaModel(), _currencyList, spname);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }
    }
}