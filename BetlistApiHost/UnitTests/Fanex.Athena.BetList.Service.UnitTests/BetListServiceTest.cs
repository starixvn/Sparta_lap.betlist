namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System.Collections.Generic;
    using Athena.BetList.DataAccess.Interfaces;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Athena.BetList.Service;
    using Athena.BetList.Service.Interfaces;
    using Common.DataAccess.Interfaces;
    using Common.Model.DTO;
    using Common.Utility.Services.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BetListServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private IBetListDataAccess _betlistDataAccess;

        private IBetListService _betListService;

        private ITicketHelper _ticketHelper;

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ICacheRefAdapter _cacheRefAdapter;

        [OneTimeSetUp]
        public void SetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _betlistDataAccess = Substitute.For<IBetListDataAccess>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();

            _betListService = new BetListService(balanceUpDataAccess, _betlistDataAccess, _ticketHelper, _betlistHtmlBuilder, _cacheRefAdapter);

            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _betlistDataAccess = null;
            _betListService = null;
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new TicketResult();
            var metaModel = new MixParlayDetailMetaModel();
            _betlistDataAccess.GetMixParlayDetailTicket(metaModel).Returns(expectedResult);

            // Action
            var result = _betListService.GetMixParlayDetailTicket(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new SystemParlayData { Outstanding = 999 };
            var metaModel = new SystemParlayDetailMetaModel();
            _betlistDataAccess.GetSystemParlayDetail(metaModel).Returns(expectedResult);

            // Action
            var result = _betListService.GetSystemParlayDetail(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetBetListData_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new TicketResult
            {
                TotalRows = 1234
            };
            var spname = FakeHelper.Create<string>();
            var metaModel = new BetListMetaModel();
            _betlistDataAccess.GetBetListData(metaModel).Returns(expectedResult);

            // Action
            var result = _betListService.GetBetListData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}