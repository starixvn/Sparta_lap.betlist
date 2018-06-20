namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetList.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    public class BettingTicketsCenterControllerTest
    {
        private ISessionService _sessionService;

        private IBettingTicketsCenterService _service;

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ICacheRefAdapter _cacheRefAdapter;

        private ITicketHelper _ticketHelper;

        private BettingTicketsCenterController _controller;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _service = Substitute.For<IBettingTicketsCenterService>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();

            _sessionService.GenerateAgentListXml(Arg.Any<int>()).Returns("<xml></xml>");
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });

            _controller = new BettingTicketsCenterController(
                _sessionService,
                _service,
                _betlistHtmlBuilder,
                _cacheRefAdapter,
                _ticketHelper);
        }

        public void FixtureTearDown()
        {
            _sessionService = null;
            _service = null;
            _betlistHtmlBuilder = null;
            _cacheRefAdapter = null;
            _ticketHelper = null;
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
        public void GetByRefno_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };

            _service.GetBetListByRefno(metaModel).Returns(new TicketResult());

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received(1).GetBetListByRefno(metaModel);
                Assert.That(result.Data, Is.Not.Null);
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
            _service.GetRunningBetList(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received(1).GetRunningBetList(metaModel);
                Assert.That(result.Data, Is.Not.Null);
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

            _service.GetBetListByRefno(metaModel).Returns(ticketResult);
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

            _service.GetBetListByRefno(metaModel).Returns(ticketResult);

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

            _service.GetBetListByRefno(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data[0].Index, Is.EqualTo(1));
                Assert.That(result.Data[0].ExternalId, Is.EqualTo(ticket.ExternalId));
                Assert.That(result.Data[0].SystemId, Is.EqualTo(ticket.UserName));
            });
        }
    }
}