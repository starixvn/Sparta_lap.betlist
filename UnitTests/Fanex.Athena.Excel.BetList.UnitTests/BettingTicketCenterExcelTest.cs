namespace Fanex.Athena.Excel.BetList.UnitTests
{
    using Athena.Model.BetList.DTO;
    using Athena.Model.Core.DataDomain;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    public class BettingTicketCenterExcelTest
    {
        private const int NumberOfColumns = 12;

        private ICacheRefAdapter _cacheRefAdapter;
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        private ICellStyle _oddStyle;
        private ICellStyle _evenStyle;

        [SetUp]
        public void Setup()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();

            _oddStyle = Substitute.For<ICellStyle>();
            _oddStyle.FillBackgroundColor = 2;
            _builder.OddCellStyle.Returns(_oddStyle);

            _evenStyle = Substitute.For<ICellStyle>();
            _evenStyle.FillBackgroundColor = 1;
            _builder.EvenCellStyle.Returns(_evenStyle);

            _builder.CreateCellStyle().Returns(_oddStyle, _evenStyle, _oddStyle, _evenStyle);
            _builder.When(x => x.BuildWorksheet(Arg.Any<string>(), Arg.Any<short>())).DoNotCallBase();
        }

        [TearDown]
        public void TearDown()
        {
            _cacheRefAdapter = null;
            _ticketHelper = null;
            _builder = null;
        }

        [Test]
        public void Export_WhenCall_BuildWorkBook()
        {
            // Arrange
            var excel = CreateExcel(new BettingTicketCenterExcelModel());
            excel.When(x => x.Build(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            var result = excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received(1).Build(_builder);
                Assert.That(result, Is.EqualTo(workbook));
            });
        }

        [Test]
        public void Export_NoData_BuildWorkBookWithNoInfoRow()
        {
            // Arrange
            var excel = CreateExcel(new BettingTicketCenterExcelModel { Data = new TicketResult() });
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).BuildWorksheet(ViewResources.BettingTicketCenter, NumberOfColumns);
                excel.Received(1).AddTitle(_builder);
                excel.Received(1).AddHeader(_builder);
                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoInfo),
                    Arg.Any<ICellStyle>());
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 2 && x.LastRow == 2
                    && x.FirstColumn == 0 && x.LastColumn == NumberOfColumns - 1));
            });
        }

        [Test]
        public void Export_AddTitle_AddCorrectly()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult(),
                Title = FakeHelper.Create<string>()
            };
            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddNoInfoRow(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == excelModel.Title),
                    Arg.Any<ICellStyle>());
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 0 && x.LastRow == 0
                    && x.FirstColumn == 0 && x.LastColumn == NumberOfColumns - 1));
            });
        }

        [Test]
        public void Export_AddHeader_AddCorrectly()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult()
            };
            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddNoInfoRow(_builder)).DoNotCallBase();
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns).AddCell(Arg.Any<HSSFRichTextString>(), Arg.Any<ICellStyle>());
                _builder.Received(NumberOfColumns).SetColumnWidth(Arg.Any<int>(), Arg.Any<short>());
            });
        }

        [Test]
        public void Export_AddHeader_IsAllowComm_AddCustomerWLColumnNameCorrectly()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult(),
                CustomerCommVisible = true
            };
            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddNoInfoRow(_builder)).DoNotCallBase();
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.WinLoss + "/" + ViewResources.Comm),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void Export_AddHeader_IsNotAllowComm_AddCustomerWLColumnNameCorrectly()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult(),
                CustomerCommVisible = false
            };
            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddNoInfoRow(_builder)).DoNotCallBase();
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.WinLoss),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void Export_AssignSheetsData_CallRelevantMethods()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult(),
                ShowTotal = true
            };
            excelModel.Data.MainTicketList.Add(new LrfTicket());

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            excel.When(x => x.AddFooter(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received(1).AddTitle(_builder);
                excel.Received(1).AddHeader(_builder);
                excel.Received(1).AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>());
                excel.Received(1).AddFooter(_builder);

                _builder.Received().GetBetList();
            });
        }

        [Test]
        public void Export_AssignSheetsData_IsNotShowFooter_NotCallAddFooterMethod()
        {
            // Arrange
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = new TicketResult(),
                ShowTotal = false
            };
            excelModel.Data.MainTicketList.Add(new LrfTicket());

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            excel.When(x => x.AddFooter(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            excel.DidNotReceive().AddFooter(_builder);
        }

        [Test]
        public void Export_AddFooter_IsNotShowTotalData_AddEmptyCells()
        {
            // Arrange
            var ticket = new TicketResult
            {
                SumStake = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>()
            };
            ticket.MainTicketList.Add(new LrfTicket());
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticket,
                ShowTotal = true
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);

            // Action
            excel.Export();

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns).AddCell(Arg.Any<HSSFRichTextString>(), Arg.Any<ICellStyle>());
                _builder.DidNotReceive().AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == ticket.SumStake.ToString(FormatKey.NumberFormat)
                    || x.String == (ticket.SumWinLoss + ticket.SumPlayerComm).ToString(FormatKey.NumberFormat)
                    || x.String == (ticket.SumLicenseeWL + ticket.SumComm).ToString(FormatKey.NumberFormat)),
                    Arg.Any<ICellStyle>());

                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == lastRow && x.LastRow == lastRow
                    && x.FirstColumn == 0 && x.LastColumn == 7),
                    Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void Export_AddFooter_ShowSumStake_AddSumStakeCell()
        {
            // Arrange
            var ticket = new TicketResult
            {
                SumStake = FakeHelper.Create<decimal>()
            };
            ticket.MainTicketList.Add(new LrfTicket());
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticket,
                ShowTotal = true,
                ShowSumStake = true
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ticket.SumStake.ToString(FormatKey.NumberFormat)),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void Export_AddFooter_ShowSumWinLoss_AddSumWinLossCell()
        {
            // Arrange
            var ticket = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>()
            };
            ticket.MainTicketList.Add(new LrfTicket());
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticket,
                ShowTotal = true,
                ShowWinLoss = true
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == (ticket.SumWinLoss + ticket.SumPlayerComm).ToString(FormatKey.NumberFormat)),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void Export_AddFooter_ShowSumLicenseeWl_AddSumLicenseeWlCell()
        {
            // Arrange
            var ticket = new TicketResult
            {
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
            };
            ticket.MainTicketList.Add(new LrfTicket());
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticket,
                ShowTotal = true,
                ShowLicenseeWl = true
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();
            excel.When(x => x.AsignData(_builder, Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == (ticket.SumLicenseeWL + ticket.SumComm).ToString(FormatKey.NumberFormat)),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void Export_AddPlatfromCell_IndexIsEven_BuildCorrectCellWithEvenCellStyle()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetFrom = "a",
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won
            };
            tickets.MainTicketList.Add(ticket);
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = tickets
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            _ticketHelper.Index.Returns(0);            

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == PlatformType.GetPlatformName(ticket.BetFrom)),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center 
                && x.FillBackgroundColor == _evenStyle.FillBackgroundColor));
        }

        [Test]
        public void Export_AddPlatfromCell_IndexIsOdd_BuildCorrectCellWithOddCellStyle()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetFrom = "g",
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won
            };
            tickets.MainTicketList.Add(ticket);
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = tickets
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            _ticketHelper.Index.Returns(1);
        
            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == PlatformType.GetPlatformName(ticket.BetFrom)),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center
                && x.FillBackgroundColor == _oddStyle.FillBackgroundColor));
        }

        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, false)]
        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, true)]
        public void Export_AddLiveCell_BuildLiveCell(BetType bettype, int liveHomeScore, bool isLive)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)bettype,
                Status = BetStatus.Won,
                IsLive = isLive,
                LiveHomeScore = liveHomeScore
            };
            tickets.MainTicketList.Add(ticket);
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = tickets
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.Live),
                Arg.Any<ICellStyle>());
        }

        [TestCase(BetType.MixParlay, 0, false)]
        [TestCase(BetType.MixParlay, 0, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, false)]
        public void Export_AddLiveCell_BuildNonLiveCell(BetType bettype, int liveHomeScore, bool isLive)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)bettype,
                Status = BetStatus.Won,
                IsLive = isLive,
                LiveHomeScore = liveHomeScore
            };
            tickets.MainTicketList.Add(ticket);
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = tickets
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoneLive),
                Arg.Any<ICellStyle>());
        }

        public void Export_AddLiveCell_BuildMixLiveAndNoneLiveCell()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.MixParlay,
                Status = BetStatus.Won,
                LiveHomeScore = LiveStatusTypes.All
            };
            tickets.MainTicketList.Add(ticket);
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = tickets
            };

            var excel = CreateExcel(excelModel);
            excel.When(x => x.AddTitle(_builder)).DoNotCallBase();
            excel.When(x => x.AddHeader(_builder)).DoNotCallBase();

            var workbook = Substitute.For<IWorkbook>();
            _builder.GetBetList().Returns(workbook);

            // Action
            excel.Export();

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.MixLiveAndNoneLive),
                Arg.Any<ICellStyle>());
        }

        private BettingTicketCenterExcel CreateExcel(BettingTicketCenterExcelModel excelModel)
        {
            var betListExcel = Substitute.ForPartsOf<BettingTicketCenterExcel>(
                _cacheRefAdapter,
                _ticketHelper,
                excelModel);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            return betListExcel;
        }
    }
}