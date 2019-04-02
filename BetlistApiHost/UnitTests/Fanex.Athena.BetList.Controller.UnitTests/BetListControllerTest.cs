namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Utility.Collections;
    using Common.Utility.Helpers;
    using Common.Utility.Services.Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using Model.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class BetListControllerTest : BaseControllerHelperTest
    {
        private ICacheRefAdapter _cacheRefAdapter;

        private IBetListService _betListService;

        [SetUp]
        public void SetUp()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _betListService = Substitute.For<IBetListService>();
        }

        [TearDown]
        public void TearDown()
        {
            _cacheRefAdapter = null;
            _betListService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectString()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index();

                // Assert
                Assert.That(result, Is.EqualTo("Betlist Api default page"));
            }
        }

        [Test]
        public async Task GetBetList_NoMainTicket_ReturnLeagueNameIsEmpty()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                var ticketResult = new TicketResult
                {
                    SumWinLoss = FakeHelper.Create<decimal>(),
                    SumPlayerComm = FakeHelper.Create<decimal>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                    SumBetEffective = FakeHelper.Create<decimal>()
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _betListService.BuildTicketTextList(ticketResult, metaModel, isShowPlayerComm: metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (await controller.GetBetList(metaModel)).Data as BetListResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Main, Is.EqualTo(ticketText));
                    Assert.That(result.Total.SumWinLoss, Is.EqualTo(ticketResult.SumWinLoss + ticketResult.SumPlayerComm));
                    Assert.That(result.Total.SumLicenseeWl, Is.EqualTo(ticketResult.SumLicenseeWL + ticketResult.SumComm));
                    Assert.That(result.Total.SumBetEffective, Is.EqualTo(ticketResult.SumBetEffective));
                    Assert.That(result.Total.SumStake, Is.Zero);
                    Assert.That(result.LeagueName, Is.Empty);

                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task GetBetList_HasMainTicket_ReturnCorrectLeagueName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                var ticketResult = new TicketResult
                {
                    SumWinLoss = FakeHelper.Create<decimal>(),
                    SumPlayerComm = FakeHelper.Create<decimal>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                    SumBetEffective = FakeHelper.Create<decimal>(),

                    MainTicketList = new SortableList<LrfTicket> { ticket }
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _betListService.BuildTicketTextList(ticketResult, metaModel, isShowPlayerComm: metaModel.HasPlayerCom).Returns(ticketText);

                var leagueName = FakeHelper.Create<string>();
                _betListService.GetLeagueName(ticket.LeagueId).Returns(leagueName);

                // Action
                var result = (await controller.GetBetList(metaModel)).Data as BetListResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Main, Is.EqualTo(ticketText));
                    Assert.That(result.Total.SumWinLoss, Is.EqualTo(ticketResult.SumWinLoss + ticketResult.SumPlayerComm));
                    Assert.That(result.Total.SumLicenseeWl, Is.EqualTo(ticketResult.SumLicenseeWL + ticketResult.SumComm));
                    Assert.That(result.Total.SumBetEffective, Is.EqualTo(ticketResult.SumBetEffective));
                    Assert.That(result.Total.SumStake, Is.EqualTo(Math.Round(ticket.Stake, 2)));
                    Assert.That(result.LeagueName, Is.EqualTo(leagueName));

                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1, 20, 50, 1, 3)]
        [TestCase(2, 30, 60, 31, 2)]
        [TestCase(3, 10, 60, 21, 6)]
        public async Task GetPagingData_WhenCall_ReturnCorrectData(int pageIndex, int pageSize, int totalRows, int startIndex, int totalPage)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();
                metaModel.PageIndex = pageIndex;
                metaModel.PageSize = pageSize;

                var ticketResult = new TicketResult { TotalRows = totalRows };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult, metaModel, startIndex, isShowPlayerComm: metaModel.HasPlayerCom).Returns(ticketText.ToList());

                // Action
                var result = (await controller.GetBetListPaging(metaModel)).Data;
                var page = result.GetType().GetProperty("page").GetValue(result, null);
                var total = result.GetType().GetProperty("total").GetValue(result, null);
                var records = result.GetType().GetProperty("records").GetValue(result, null);
                var rows = result.GetType().GetProperty("rows").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(page, Is.EqualTo(pageIndex));
                    Assert.That(total, Is.EqualTo(totalPage));
                    Assert.That(records, Is.EqualTo(totalRows));
                    Assert.That(rows, Is.EqualTo(ticketText));
                    AssertClearData(ticketResult);
                });
            }
        }

        [TestCase(1.123, 2.345, "3.47")]
        [TestCase(1.123, 2.34, "3.46")]
        [TestCase(-1.123, -2.345, "<span class='negative-number'>-3.47</span>")]
        public async Task GetPagingData_WhenCall_ReturnCorrectLicenseeWLText(decimal sumLicenseeWL, decimal sumComm, string expectedLicenseeWLText)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                var ticketResult = new TicketResult
                {
                    SumLicenseeWL = sumLicenseeWL,
                    SumComm = sumComm,
                    SumWinLoss = FakeHelper.Create<decimal>(),
                    SumPlayerComm = FakeHelper.Create<decimal>()
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult, metaModel, Arg.Any<int>(), isShowPlayerComm: metaModel.HasPlayerCom)
                    .Returns(ticketText.ToList());

                var expectedWinloss = NumberHelper.DecFormat(ticketResult.SumWinLoss + ticketResult.SumPlayerComm, 2);

                // Action
                var result = (await controller.GetBetListPaging(metaModel)).Data;
                var userdata = result.GetType().GetProperty("userdata").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(userdata.GetType().GetProperty("WinLoss").GetValue(userdata, null), Is.EqualTo(expectedWinloss));
                    Assert.That(userdata.GetType().GetProperty("LicenseeWL").GetValue(userdata, null), Is.EqualTo(expectedLicenseeWLText));
                });
            }
        }

        [Test]
        public async Task GetDailyBetListData_WhenCall_CallGetBetListPaging()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<DailyBetListMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2);
                _betListService.BuildTicketTextList(ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), isShowPlayerComm: metaModel.HasPlayerCom)
                    .Returns(ticketText.ToList());

                // Action
                 await controller.GetBetListPaging(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _betListService.Received().GetBetListData(metaModel);
                    _betListService.Received().BuildTicketTextList(ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), isShowPlayerComm: metaModel.HasPlayerCom);
                });
            }
        }

        [Test]
        public void GetBetList_GetWinLossByLeagueData_CallGetBetList()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<WinLossByLeagueBetListMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(Arg.Any<BetListMetaModel>()).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _betListService.BuildTicketTextList(ticketResult, Arg.Any<BetListMetaModel>(), isShowPlayerComm: metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                controller.GetWinLossByLeagueData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _betListService.Received().GetBetListData(metaModel);
                    _betListService.Received().BuildTicketTextList(ticketResult, metaModel, isShowPlayerComm: metaModel.HasPlayerCom);
                });
            }
        }

        [Test]
        public async Task ExportExcel_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListExcelMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                controller.When(x => x.Export(metaModel, ticketResult)).DoNotCallBase();

                // Action
                await controller.ExportExcel(metaModel);

                // Assert
                controller.Received().Export(metaModel, ticketResult);
            }
        }

        [Test]
        public async Task ExportExcel_HasLeagueNameInTitle_NoMainTicket_ExportCorrectTitle()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListExcelMetaModel>(x => x.Title, "(LEAGUENAME)");

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                controller.When(x => x.Export(metaModel, ticketResult)).DoNotCallBase();

                // Action
                await controller.ExportExcel(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().Export(metaModel, ticketResult);
                    _cacheRefAdapter.DidNotReceive().GetLeagueName(Arg.Any<int>());
                });
            }
        }

        [Test]
        public async Task ExportExcel_HasLeagueNameInTitle_ExportCorrectTitle()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListExcelMetaModel>(x => x.Title, "(LEAGUENAME)");

                var ticket = FakeHelper.Create<LrfTicket>();
                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket> { ticket }
                };
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _cacheRefAdapter.GetLeagueName(ticket.LeagueId).Returns(title);

                controller.When(x => x.Export(metaModel, ticketResult)).DoNotCallBase();

                // Action
                await controller.ExportExcel(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().Export(metaModel, ticketResult);
                    Assert.That(metaModel.Title, Is.EqualTo(title));
                });
            }
        }

        [Test]
        public async Task ExportExcelDailyBetList_WhenCall_CallExportExcel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<DailyBetListMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                controller.When(x => x.Export(Arg.Any<DailyBetListMetaModel>(), ticketResult)).DoNotCallBase();

                // Action
                await controller.ExportExcelDailyBetList(metaModel);

                // Assert
                controller.Received().Export(metaModel, ticketResult);
            }
        }

        [Test]
        public async Task ExportExcelWinlossByLeague_WhenCall_CallExportExcel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<WinLossByLeagueBetListMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetBetListData(metaModel).Returns(ticketResult);

                controller.When(x => x.Export(Arg.Any<WinLossByLeagueBetListMetaModel>(), ticketResult)).DoNotCallBase();

                // Action
                await controller.ExportExcelWinlossByLeague(metaModel);

                // Assert
                controller.Received().Export(metaModel, ticketResult);
            }
        }

        private BetListController CreateController()
        {
            return Substitute.ForPartsOf<BetListController>(_cacheRefAdapter, _betListService);
        }
    }
}