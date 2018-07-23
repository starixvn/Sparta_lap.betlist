namespace Fanex.Athena.Excel.BetList.Builders.UnitTests
{
    using Athena.Model.BetList.DTO;
    using Athena.Model.Core.DataDomain;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
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
        private const int ColumnTotalStart = 6;

        private ICacheRefAdapter _cacheRefAdapter;
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        private ICellStyle _oddStyle;
        private ICellStyle _evenStyle;

        private IBettingTicketCenterExcelBuilder _excelBuilder;

        [SetUp]
        public void Setup()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();

            _excelBuilder = Substitute.ForPartsOf<BettingTicketCenterExcelBuilder>(
                _builder,
                _cacheRefAdapter,
                _ticketHelper);

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
        public void AddNoInfoRow_WhenCall_WorkCorrectly()
        {
            // Arrange
            var titleStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(titleStyle);

            // Action
            _excelBuilder.AddNoInfoRow();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(titleStyle.Alignment, Is.EqualTo(HorizontalAlignment.Center));

                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoInfo),
                    titleStyle);
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 2 && x.LastRow == 2
                    && x.FirstColumn == 0 && x.LastColumn == NumberOfColumns - 1));
            });
        }

        [Test]
        public void AddTitle_WhenCall_WorkCorrectly()
        {
            // Arrange
            const string Title = "sddflghdlkfjghldjfg";
            var font = Substitute.For<IFont>();
            _builder.CreateFont().Returns(font);
            var titleStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(titleStyle);

            // Action
            _excelBuilder.AddTitle(Title);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(font.Boldweight, Is.EqualTo((short)FontBoldWeight.Bold));
                Assert.That(font.Color, Is.EqualTo(HSSFColor.Orange.Index));

                Assert.That(titleStyle.VerticalAlignment, Is.EqualTo(VerticalAlignment.Center));
                titleStyle.Received(1).SetFont(font);

                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == Title),
                    titleStyle);
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 0 && x.LastRow == 0
                    && x.FirstColumn == 0 && x.LastColumn == NumberOfColumns - 1));
            });
        }

        [TestCase(false, "Win Loss")]
        [TestCase(true, "Win Loss/Comm")]
        public void AddHeader_WhenCall_WorkCorrectly(bool isShowCustomerComm, string winLossHeader)
        {
            // Arrange
            var cellStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(cellStyle);
            var captions = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.TransTime,
                ViewResources.Choice,
                ViewResources.Odds,
                ViewResources.Stake,
                winLossHeader,
                ViewResources.LicenseeWL + "/" + ViewResources.Comm,
                ViewResources.Status,
                ViewResources.Platform,
                ViewResources.Live
            };

            // Action
            _excelBuilder.AddHeader(isShowCustomerComm);

            // Assert
            Assert.Multiple(() =>
            {
                for (int i = 0; i < captions.Length; i++)
                {
                    _builder.Received(1).AddCell(
                        Arg.Is<HSSFRichTextString>(x => x.String == captions[i]),
                        cellStyle);
                }
            });
        }

        [Test]
        public void AddFooter_WhenCall_WorkCorrectly()
        {
            // Arrange
            const decimal SumStake = 12.12m;
            const decimal SumWinLoss = 23.23m;
            const decimal SumLicenseeWL = 34.34m;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(SumStake, SumWinLoss, SumLicenseeWL);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(ColumnTotalStart).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.Total), Arg.Any<ICellStyle>());
                _builder.Received(NumberOfColumns - ColumnTotalStart - 3).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumStake.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumWinLoss.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumLicenseeWL.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void AddFooter_ShowSumStake_AddSumStakeCell()
        {
            // Arrange
            decimal? sumStake = null;
            const decimal SumWinLoss = 23.23m;
            const decimal SumLicenseeWL = 34.34m;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(sumStake, SumWinLoss, SumLicenseeWL);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 2).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumWinLoss.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumLicenseeWL.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void AddFooter_ShowSumWinLoss_AddSumWinLossCell()
        {
            // Arrange
            decimal? sumWinLoss = null;
            const decimal SumStake = 23.23m;
            const decimal SumLicenseeWL = 34.34m;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(SumStake, sumWinLoss, SumLicenseeWL);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 2).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumStake.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumLicenseeWL.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void AddFooter_ShowSumLicenseeWL_AddSumLicenseeWLCell()
        {
            // Arrange
            decimal? sumLicenseeWL = null;
            const decimal SumStake = 23.23m;
            const decimal SumWinLoss = 34.34m;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(SumStake, SumWinLoss, sumLicenseeWL);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 2).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumStake.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumWinLoss.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void AssignRowData_AddPlatfromCell_IndexIsOdd_BuildCorrectCellWithOddCellStyle()
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

            _ticketHelper.Index.Returns(0);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, true, 1);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == PlatformType.GetPlatformName(ticket.BetFrom)),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center
                && x.FillBackgroundColor == _evenStyle.FillBackgroundColor));
        }

        [Test]
        public void AssignRowData_AddPlatfromCell_IndexIsEven_BuildCorrectCellWithOddCellStyle()
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

            _ticketHelper.Index.Returns(1);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, true, 1);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == PlatformType.GetPlatformName(ticket.BetFrom)),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center
                && x.FillBackgroundColor == _oddStyle.FillBackgroundColor));
        }

        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, false)]
        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, true)]
        public void AssignRowData_AddLiveCell_BuildLiveCell(BetType bettype, int liveHomeScore, bool isLive)
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

            _ticketHelper.Index.Returns(2);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, false, 2);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.Live),
                Arg.Any<ICellStyle>());
        }

        [TestCase(BetType.MixParlay, 0, false)]
        [TestCase(BetType.MixParlay, 0, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, false)]
        public void AssignRowData_AddLiveCell_BuildNonLiveCell(BetType bettype, int liveHomeScore, bool isLive)
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

            _ticketHelper.Index.Returns(3);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, false, 2);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoneLive),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void AssignRowData_AddLiveCell_BuildMixLiveAndNoneLiveCell()
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

            _ticketHelper.Index.Returns(4);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, true, 3);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.MixLiveAndNoneLive),
                Arg.Any<ICellStyle>());
        }
    }
}