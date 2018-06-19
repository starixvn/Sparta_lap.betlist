namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    public class BettingTicketsCenterControllerTest
    {
        private ISessionService _sessionService;

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IBettingTicketsCenterDataAccess _dataAccess;

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ICacheRefAdapter _cacheRefAdapter;

        private ITicketHelper _ticketHelper;

        private IAuthorizationService _authorizationService;

        private BettingTicketsCenterController _controller;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _dataAccess = Substitute.For<IBettingTicketsCenterDataAccess>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _authorizationService = Substitute.For<IAuthorizationService>();

            var balanceUp = new BalanceUp { BalanceUpDay = new DateTime(2018, 1, 1), MoveBetsDay = new DateTime(2018, 1, 2) };
            _balanceUpDataAccess.GetBalanceUp().Returns(balanceUp);
            _sessionService.GenerateAgentListXml(Arg.Any<int>()).Returns("<xml></xml>");

            _controller = new BettingTicketsCenterController(
                _sessionService,
                _authorizationService,
                _balanceUpDataAccess,
                _dataAccess,
                _betlistHtmlBuilder,
                _cacheRefAdapter,
                _ticketHelper);
        }

        public void FixtureTearDown()
        {
            _sessionService = null;
            _balanceUpDataAccess = null;
            _dataAccess = null;
            _betlistHtmlBuilder = null;
            _cacheRefAdapter = null;
            _ticketHelper = null;
            _authorizationService = null;
            _controller = null;
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-80)]
        public void GetByRefno_MetaModelIsInvalid_ReturnNull(int refNo)
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = refNo
            };

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByRefno_NoPermission_ReturnNull()
        {
            // Arrange
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(false);

            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByRefno_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };

            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(true);
            _dataAccess.GetBetListByRefno(metaModel).Returns(new TicketResult());

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetBetListByRefno(metaModel);
                Assert.That(result.Main, Is.Not.Null);
            });
        }

        [Test]
        public void GetByRunning_DateRangeIsInvalid_ReturnNull()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetByRunning_NoProduct_ReturnNull(string productCode)
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = productCode
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByRunning_InvalidCurrency_ReturnNull()
        {
            // Arrange
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 2
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByRunning_NoPermission_ReturnNull()
        {
            // Arrange
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(false);
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByRunning_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1
            };

            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(true);
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var ticketResult = new TicketResult
            {
                SumStake = FakeHelper.Create<int>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetRunningBetList(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetRunningBetList(metaModel);
                Assert.That(result.Main, Is.Not.Null);
                Assert.That(result.Total.SumStake, Is.EqualTo(ticketResult.SumStake));
                Assert.That(result.TotalRows, Is.EqualTo(ticketResult.TotalRows));
            });
        }

        [Test]
        public void BuildTicketText_IsNotCombinationMixParlay_ReturnNormalOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.Handicap, Status = BetStatus.Won };
            ticketResult.MainTicketList.Add(ticket);

            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(true);
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);
            _ticketHelper.ClientDetailSystemParlayFunction.Returns(string.Empty);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }

        [Test]
        public void BuildTicketText_IsCombinationMixParlay_ReturnCustomOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.CombinationMixParlay, Status = BetStatus.Lose };
            ticketResult.MainTicketList.Add(ticket);

            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(true);
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Not.Empty);
        }

        [Test]
        public void BuildTicketText_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.OverUnder,
                Status = BetStatus.Lose,
                ExternalId = FakeHelper.Create<string>(),
                UserName = FakeHelper.Create<string>()
            };
            ticketResult.MainTicketList.Add(ticket);

            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), 0).Returns(true);
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Main[0].Index, Is.EqualTo(1));
                Assert.That(result.Main[0].ExternalId, Is.EqualTo(ticket.ExternalId));
                Assert.That(result.Main[0].SystemId, Is.EqualTo(ticket.UserName));
            });
        }
    }
}