namespace Fanex.Athena.Excel.BetList.Builders.UnitTests
{
    using Athena.Model.Core.DataDomain;
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
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumStake.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumWinLoss.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
                _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == SumLicenseeWL.ToString(FormatKey.DecimalFormat)), Arg.Any<ICellStyle>());
            });
        }
    }
}