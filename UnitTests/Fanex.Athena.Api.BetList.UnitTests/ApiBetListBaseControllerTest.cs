namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections;
    using System.Collections.Generic;
    using Extension.Helper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ApiBetListBaseControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const int DefaultPageSize = 100;

        private const int DefaultPageIndex = 1;

        private FakeApiBetListBaseController _controller;

        private static IEnumerable GetPagingBetListTestCaseValue
        {
            get
            {
                yield return new TestCaseData(new
                {
                    CurrencyId = 0,
                    InPutPageIndex = 1,
                    InPutPageSize = 50,
                    TotalRows = 90,
                    MetaModelPageIndex = 1,
                    MetaModelPageSize = 50,
                    ResultTotalPage = 2,
                    SumLicenseeWL = 2,
                    SumComm = 1,
                    SumWinLoss = 3,
                    SumPlayerComm = 4,
                    SumLicenseeWLText = "3.00",
                });
                yield return new TestCaseData(new
                {
                    CurrencyId = 1,
                    InPutPageIndex = 0,
                    InPutPageSize = 0,
                    TotalRows = 100,
                    MetaModelPageIndex = 0,
                    MetaModelPageSize = DefaultPageSize,
                    ResultTotalPage = 1,
                    SumLicenseeWL = -2,
                    SumComm = 1,
                    SumWinLoss = 3,
                    SumPlayerComm = 4,
                    SumLicenseeWLText = "<span class='negative-number'>-1.00</span>",
                });
                yield return new TestCaseData(new
                {
                    CurrencyId = 1,
                    InPutPageIndex = 3,
                    InPutPageSize = 50,
                    TotalRows = 100,
                    MetaModelPageIndex = 2,
                    MetaModelPageSize = 50,
                    ResultTotalPage = 2,
                    SumLicenseeWL = 2,
                    SumComm = -3,
                    SumWinLoss = 3,
                    SumPlayerComm = 4,
                    SumLicenseeWLText = "<span class='negative-number'>-1.00</span>",
                });
            }
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<FakeApiBetListBaseController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder);

            StubLicenseeSessionData(new Model.Core.DTO.Users.LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void GetBetListResult_NoData_ReturnCorrectResult()
        {
            // Arrange
            const string UspKey = "Usp_Key";
            var metaModel = new BetListMetaModel();

            var ticketResult = new TicketResult();
            BetListService.GetBetListData(metaModel, UspKey).Returns(ticketResult);

            _controller.When(x => x.Ticket(ticketResult, metaModel, 1, false)).DoNotCallBase();
            _controller.Ticket(ticketResult, metaModel, 1, false).Returns(new List<TicketText>());

            // Action
            var result = _controller.GetBetListResult(metaModel, UspKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.LeagueName, Is.EqualTo(string.Empty));
                Assert.That(result.Total.SumWinLoss, Is.Zero);
                Assert.That(result.Total.SumLicenseeWl, Is.Zero);
                Assert.That(result.Total.SumBetEffective, Is.Zero);
                Assert.That(result.Total.SumStake, Is.Zero);
            });
        }

        [Test]
        public void GetBetListResult_HasData_ReturnCorrectResult()
        {
            // Arrange
            const string UspKey = "Usp_Key";
            var metaModel = new BetListMetaModel();

            var ticketResult = new TicketResult { SumWinLoss = 1, SumPlayerComm = 2, SumLicenseeWL = 3, SumComm = 4, SumBetEffective = 5 };
            var ticket = new LrfTicket { LeagueId = 1, Stake = 1.123m };
            ticketResult.MainTicketList.Add(ticket);
            BetListService.GetBetListData(metaModel, UspKey).Returns(ticketResult);

            const string LeagueName = "Name";
            CacheRefAdapter.GetLeagueName(ticket.LeagueId).Returns(LeagueName);

            _controller.When(x => x.Ticket(ticketResult, metaModel, 1, false)).DoNotCallBase();
            _controller.Ticket(ticketResult, metaModel, 1, false).Returns(new List<TicketText>());

            // Action
            var result = _controller.GetBetListResult(metaModel, UspKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.LeagueName, Is.EqualTo(LeagueName));
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(ticketResult.SumWinLoss + ticketResult.SumPlayerComm));
                Assert.That(result.Total.SumLicenseeWl, Is.EqualTo(ticketResult.SumLicenseeWL + ticketResult.SumComm));
                Assert.That(result.Total.SumBetEffective, Is.EqualTo(ticketResult.SumBetEffective));
                Assert.That(result.Total.SumStake, Is.EqualTo(1.12));
            });
        }

        [TestCaseSource(nameof(GetPagingBetListTestCaseValue))]
        public void GetPagingBetListResult_WhenCall_ReturnCorrectResult(dynamic testCaseData)
        {
            // Arrange
            const string UspKey = "Usp_Key";

            var metaModel = new BetListMetaModel();

            var ticketResult = new TicketResult
            {
                TotalRows = testCaseData.TotalRows,
                SumWinLoss = testCaseData.SumWinLoss,
                SumPlayerComm = testCaseData.SumPlayerComm,
                SumLicenseeWL = testCaseData.SumLicenseeWL,
                SumComm = testCaseData.SumComm
            };
            BetListService.GetBetListData(metaModel, UspKey).Returns(ticketResult);

            _controller.When(x => x.Ticket(ticketResult, metaModel, Arg.Any<int>(), false)).DoNotCallBase();
            _controller.Ticket(ticketResult, metaModel, Arg.Any<int>(), false).Returns(new List<TicketText>());

            // Action
            var result = _controller.GetPagingBetListResult(metaModel, UspKey, testCaseData.InPutPageIndex, testCaseData.InPutPageSize);

            // Assert
            var sumWinLossText = NumberHelper.DecFormat(ticketResult.SumWinLoss + ticketResult.SumPlayerComm, 2);

            Assert.Multiple(() =>
            {
                Assert.That(result.total, Is.EqualTo(testCaseData.ResultTotalPage));
                Assert.That(result.records, Is.EqualTo(ticketResult.TotalRows));
                Assert.That(result.userdata.WinLoss, Is.EqualTo(sumWinLossText));
                Assert.That(result.userdata.LicenseeWL, Is.EqualTo(testCaseData.SumLicenseeWLText));

                Assert.That(metaModel.PageIndex, Is.EqualTo(testCaseData.MetaModelPageIndex));
                Assert.That(metaModel.PageSize, Is.EqualTo(testCaseData.MetaModelPageSize));
            });
        }
    }
}