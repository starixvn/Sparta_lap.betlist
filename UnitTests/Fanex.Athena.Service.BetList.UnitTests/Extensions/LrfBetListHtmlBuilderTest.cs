namespace Fanex.Athena.Service.BetList.UnitTests.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using BetList.Extensions;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;

    public class LrfBetListHtmlBuilderTest
    {
        private BetListHTMLBuilder _betListBuilder;

        private ITicketHelper _ticketHelper;

        /// <summary>
        /// Return test cases to test special ticket - include Bet type 'P2PGames' & 'MiniGames' and Sport type 'Keno'
        /// </summary>
        private static IEnumerable StatusNormalTextTestCase
        {
            get
            {
                yield return new TestCaseData(BetType.P2PGames, SportType.AllBet, BetStatus.Running, CoreBetList.running);
                yield return new TestCaseData(BetType.MiniGames, SportType.MicroGaming, BetStatus.Reject, CoreBetList.reject);
                yield return new TestCaseData(BetType.CorrectScore, SportType.Keno, BetStatus.Won, CoreBetList.won);
                yield return new TestCaseData(BetType.P2PGames, SportType.Keno, BetStatus.Lose, CoreBetList.lose);
                yield return new TestCaseData(BetType.Handicap, SportType.Keno, BetStatus.Draw, CoreBetList.draw);
                yield return new TestCaseData(BetType.Ft1X2, SportType.Keno, BetStatus.Waiting, CoreBetList.waiting);
                yield return new TestCaseData(BetType.MixParlay, SportType.Keno, BetStatus.Void, CoreBetList.voided);
                yield return new TestCaseData(BetType.FirstHalFirsGoalLastGoal, SportType.Keno, BetStatus.Refund, CoreBetList.refund);
                yield return new TestCaseData(BetType.MiniGames, SportType.Keno, string.Empty, string.Empty);
            }
        }

        private static IEnumerable CasinoHandHistoryParamTestCase
        {
            get
            {
                yield return new TestCaseData(
                    new Dictionary<string, object> { { "UserName", string.Empty }, { "ExternalId", string.Empty }, { "CustId", 0 } },
                    new Dictionary<string, object> { { "SystemId", "SystemId model" }, { "ExternalId", "ExternalId model" }, { "CustId", 1 } },
                    new Dictionary<string, object> { { "SystemId", "SystemId model" }, { "ExternalId", "ExternalId model" }, { "CustId", 1 } });
                yield return new TestCaseData(
                    new Dictionary<string, object> { { "UserName", "SystemId ticket" }, { "ExternalId", "ExternalId ticket" }, { "CustId", 2 } },
                    new Dictionary<string, object> { { "SystemId", "SystemId model" }, { "ExternalId", "ExternalId model" }, { "CustId", 1 } },
                    new Dictionary<string, object> { { "SystemId", "SystemId ticket" }, { "ExternalId", "ExternalId ticket" }, { "CustId", 2 } });
            }
        }

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _betListBuilder = Substitute.ForPartsOf<BetListHTMLBuilder>();

            _ticketHelper = Substitute.For<ITicketHelper>();
        }

        [Test]
        public void BuildLrfStatus_CasinoGamesBetType_ReturnCorrectFormat()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                RefNo = "RefTest",
                BetTypeId = (int)BetType.CasinoGames,
                Status = string.Empty
            };

            var expectedHtmlId = "casino-hand-history-" + ticket.RefNo;

            // Action
            var result = _betListBuilder.BuildLrfStatus(ticket, _ticketHelper, new List<ITicketData>(), new BetListMetaModel());

            // Assert
            StringAssert.Contains(expectedHtmlId, result);
        }

        [TestCaseSource(nameof(CasinoHandHistoryParamTestCase))]
        public void BuildLrfStatus_CasinoGamesBetType_GetCasinoHandHistoryParam_ReturnCorrectFormat(
            Dictionary<string, object> ticket, 
            Dictionary<string, object> model, 
            Dictionary<string, object> expected)
        {
            // Arrange
            var defaultTicket = new LrfTicket
            {
                RefNo = "RefTest",
                BetTypeId = (int)BetType.CasinoGames,
                Status = BetStatus.Lose,
                UserName = (string)ticket["UserName"],
                ExternalId = (string)ticket["ExternalId"],
                CustId = (int)ticket["CustId"]
            };
            var defaultModel = new BetListMetaModel
            {
                CurrencyName = "USD",
                BaseCurrency = "USD",
                FromDate = new System.DateTime(2018, 1, 1),
                ToDate = new System.DateTime(2018, 1, 2),
                SystemId = (string)model["SystemId"],
                ExternalId = (string)model["ExternalId"],
                CustId = (int)model["CustId"]
            };
            var expectedHtml = $"?RefNo=RefTest&FromDate={defaultModel.FromDate}&ToDate={defaultModel.ToDate}&SystemId={expected["SystemId"]}"
                + $"&ExternalId={expected["ExternalId"]}&CustId={expected["CustId"]}&CurrencyId=0&CurrencyName=USD&BaseCurrency=usd";

            // Action
            var result = _betListBuilder.BuildLrfStatus(defaultTicket, _ticketHelper, new List<ITicketData>(), defaultModel);

            // Assert
            StringAssert.Contains(expectedHtml, result);
        }

        [TestCaseSource(nameof(StatusNormalTextTestCase))]
        public void BuildLrfStatus_SpecicalTicket_ReturnStatusNormalText(BetType betType, SportType sportType, string status, string expectedStatus)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                RefNo = "RefTest",
                BetTypeId = (int)betType,
                Status = status,
                SportTypeId = (int)sportType
            };

            var expectedHtml = $"<div class='status'>{expectedStatus}</div>";

            // Action
            var result = _betListBuilder.BuildLrfStatus(ticket, _ticketHelper, new List<ITicketData>(), new BetListMetaModel());

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        [TestCase(BetType.Win)]
        [TestCase(BetType.WinPlace)]
        public void BuildLicenseeWL_RacingRunningTicket_SeparateWLandComm(BetType betType)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)betType,
                Status = BetStatus.Running
            };

            const string ExpectedHtml = "<div class ='lic-win-loss'>-<br/>-</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(BetType.BetTradeBought)]
        [TestCase(BetType.CashOutBought)]
        public void BuildLicenseeWL_BoughtTicket_PositiveValue_SeparateWLandComm(BetType betType)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)betType,
                Status = BetStatus.Won,
                Comm = 1,
                LicenseeWL = 2
            };

            const string ExpectedHtml = "<div class ='lic-win-loss'>2.00<br/>1.00</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(18000)]
        [TestCase(18007)]
        public void BuildLicenseeWL_ColossusBetTicket_NegativeValue_SeparateWLandComm(BetType betType)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)betType,
                Status = BetStatus.Won,
                Comm = 1,
                LicenseeWL = -2
            };

            const string ExpectedHtml = "<div class ='lic-win-loss'><span class='negative-number'>-2.00</span><br/>1.00</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        public void BuildLicenseeWL_RemianingRunningTicket_CombineWLandCom()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Running,
                Comm = 1,
                LicenseeWL = 3
            };

            const string ExpectedHtml = "-";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(1, 3, "4.00")]
        [TestCase(-1, -3, "<span class='negative-number'>-4.00</span>")]
        public void BuildLicenseeWL_RemianingTicket_CombineWLandCom(decimal winloss, decimal comm, string expected)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won,
                Comm = comm,
                LicenseeWL = winloss
            };

            var expectedHtml = $"<div class='lic-win-loss'>{expected}</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void BuildBetEffective_WhenCall_ReturnCorrectFormat()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetEffective = 3
            };

            const string ExpectedHtml = "<div class='underdog'>3.00</div>";

            // Action
            var result = _betListBuilder.BuildBetEffective(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(true, 1, 1, "-")]
        [TestCase(false, 2, 1, "<div class='underdog'>2.00</div><div class='underdog'>1.00</div>")]
        public void BuildCustomerWL_ShowPlayerComm_ReturnCorrectFormat(bool isRunningTicket, decimal winloss, decimal playerComm, string expected)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                Winlost = winloss,
                PlayerComm = playerComm,
                Status = isRunningTicket ? BetStatus.Running : BetStatus.Won,
            };

            // Action
            var result = _betListBuilder.BuildCustomerWL(ticket, true);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(true, 1, "-")]
        [TestCase(false, 2, "<div class='underdog'>2.00</div>")]
        public void BuildCustomerWL_NoShowPlayerComm_ReturnCorrectFormat(bool isRunningTicket, decimal winloss, string expected)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                Winlost = winloss,
                Status = isRunningTicket ? BetStatus.Running : BetStatus.Won,
            };

            // Action
            var result = _betListBuilder.BuildCustomerWL(ticket, false);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}