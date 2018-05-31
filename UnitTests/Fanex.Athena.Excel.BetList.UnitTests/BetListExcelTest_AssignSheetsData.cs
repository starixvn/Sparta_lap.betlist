using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.BetList.UnitTests
{
    [TestFixture]
    internal class BetListExcelTest_AssignSheetdata
    {
        private ICacheRefAdapter _cacheRefAdapter;
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();
        }

        [Test]
        public void AssignSheetsData_ShowSumStake_AddFooterSumStake()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(isShowSumStake: true);
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 99 });
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 100 });

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            _builder.Received(1).AddCell(
                    Arg.Is<IRichTextString>(x => x.String == "199.00"),
                    Arg.Any<ICellStyle>());
        }

        [Test]
        public void AssignSheetsData_ShowWinLoss_AddFooterWinLoss()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(showingOption: new ShowingOption { GrandTotalVisible = true });
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = new TicketResult
            {
                SumWinLoss = 100,
                SumPlayerComm = 0.88m,
                SumLicenseeWL = 200,
                SumComm = 0.99m,
                SumBetEffective = 999
            };
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 99 });
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 100 });

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            Assert.Multiple(() =>
            {
                // Sum customerWL
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "100.88"),
                        Arg.Any<ICellStyle>());

                // Sum LicenseeWL
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "200.99"),
                        Arg.Any<ICellStyle>());

                // SumBetEffective
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "999.00"),
                        Arg.Any<ICellStyle>());
            });
        }

        private BetListExcel CreateBetListExcel(
            bool isShowSumStake = false,
            ShowingOption showingOption = null,
            int[] hiddenColumnIndexes = null)
        {
            var betListExcel = Substitute.ForPartsOf<BetListExcel>(
                _cacheRefAdapter,
                Substitute.For<IAppSettings>(),
                _ticketHelper,
                showingOption ?? new ShowingOption(),
                LicenseeIdentity.OneTwoBet,
                hiddenColumnIndexes ?? new int[] { },
                isShowSumStake);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            betListExcel.InitializeFonts(_builder);
            return betListExcel;
        }
    }
}