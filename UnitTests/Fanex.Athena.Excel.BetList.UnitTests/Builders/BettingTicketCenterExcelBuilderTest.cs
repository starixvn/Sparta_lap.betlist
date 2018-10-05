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

        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        private ICellStyle _oddStyle;
        private ICellStyle _evenStyle;

        private IBettingTicketCenterExcelBuilder _excelBuilder;

        [SetUp]
        public void Setup()
        {
            var cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();

            _excelBuilder = Substitute.ForPartsOf<BettingTicketCenterExcelBuilder>(
                _builder,
                cacheRefAdapter,
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
            const decimal SumPlayerComm = 34.34m;
            const decimal SumLicenseeWL = 45.45m;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(SumStake, SumWinLoss, SumPlayerComm, SumLicenseeWL);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(ColumnTotalStart).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.Total), Arg.Any<ICellStyle>());
                _builder.Received(NumberOfColumns - ColumnTotalStart - 3).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumStake.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == SumWinLoss.ToString(FormatKey.DecimalFormat) + "\n" + SumPlayerComm.ToString(FormatKey.DecimalFormat)),
                    Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumLicenseeWL.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }

        [TestCase(45698, "45,698.00")]
        [TestCase(0, "0.00")]
        [TestCase(-654321.123, "-654,321.12")]
        public void AddFooter_ShowSumStake_AddSumStakeCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(original, null, null, null);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
            });
        }

        [TestCase(0.996, "1.00")]
        [TestCase(0, "0.00")]
        [TestCase(-0.99, "-0.99")]
        public void AddFooter_ShowSumWinLoss_AddSumWinLossCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(null, original, null, null);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
            });
        }

        [TestCase(0.996, 0, "1.00\n0.00")]
        [TestCase(0, null, "0.00")]
        [TestCase(-0.99, 0.111, "-0.99\n0.11")]
        public void AddFooter_ShowSumPlayerComm_AddSumWinLossCell(decimal originalWL, decimal? originalComm, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(null, originalWL, originalComm, null);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
            });
        }

        [TestCase(12.345, "12.35")]
        [TestCase(0, "0.00")]
        [TestCase(-654321, "-654,321.00")]
        public void AddFooter_ShowSumLicenseeWL_AddSumLicenseeWLCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(null, null, null, original);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(NumberOfColumns - ColumnTotalStart - 1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
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
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Live, false)]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Live, true)]
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
        [TestCase(BetType.CombinationMixParlay, 0, false)]
        [TestCase(BetType.CombinationMixParlay, 0, true)]
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
                LiveHomeScore = LiveStatusTypes.Mix
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