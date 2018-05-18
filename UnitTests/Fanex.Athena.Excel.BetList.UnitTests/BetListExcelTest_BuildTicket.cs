using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.BetList.UnitTests
{
    [TestFixture]
    internal class BetListExcelTest_BuildTicket
    {
        private ICacheRefAdapter _cacheRefAdapter;
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;
        private BetListExcel _betListExcel;

        [SetUp]
        public void Setup()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();
            _betListExcel = CreateBetListExcel();
            _betListExcel.InitializeFonts(_builder);
        }

        [Test]
        public void BuildTicket_WhenCall_CallReleventMethods()
        {
            // Arrange
            const int Index = 9000;
            _ticketHelper.Index.Returns(Index);

            var ticketResult = new TicketResult();
            var ticket = FakeHelper.Create<LrfTicket>();
            ticketResult.MainTicketList.Add(ticket);

            // Action
            _betListExcel.BuildTicket(_builder, ticketResult, ticket);

            // Assert
            Assert.Multiple(() =>
            {
                // AddIndex
                _builder.Received(1).AddCell(
                    Arg.Is<IRichTextString>(x => x.String == Index.ToString()),
                    Arg.Any<ICellStyle>());

                // AddExternalId
                _builder.Received(1).AddCell(
                    Arg.Is<IRichTextString>(x => x.String == ticket.ExternalId),
                    Arg.Any<ICellStyle>());

                // AddSystemId
                _builder.Received(1).AddCell(
                    Arg.Is<IRichTextString>(x => x.String == ticket.UserName),
                    Arg.Any<ICellStyle>());

                _builder.Received(1).AddTrans(ticket, _ticketHelper);
                _builder.Received(1).AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
                _builder.Received(1).AddOdds(ticket, ticketResult.TicketDataList, _cacheRefAdapter.GetOddsTypeName);
                _builder.Received(1).AddStake(ticket);

                // AddCustomerWL
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}\n", ticket.Winlost)));

                // AddBetEffective
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}\n", ticket.BetEffective)));

                // AddLicenseeWL
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}", ticket.LicenseeWL + ticket.Comm)));

                _builder.Received(1).AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
            });
        }

        private BetListExcel CreateBetListExcel()
        {
            var betListExcel = Substitute.ForPartsOf<BetListExcel>(
                _cacheRefAdapter,
                Substitute.For<IAppSettings>(),
                _ticketHelper,
                new ShowingOption(),
                LicenseeIdentity.OneTwoBet,
                new int[] { },
                false);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            return betListExcel;
        }
    }
}