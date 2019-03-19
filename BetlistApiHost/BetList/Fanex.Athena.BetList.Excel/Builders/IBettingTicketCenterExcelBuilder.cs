using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Excel.Builders
{
    public interface IBettingTicketCenterExcelBuilder
    {
        void CreateSheet(string sheetName, BettingTicketCenterExcelOption option);

        IWorkbook GetWorkbook();

        void AddFooter(decimal? sumStake, decimal? sumWinLoss, decimal? sumPlayerComm, decimal? sumLicenseeWL);

        void AddHeader(bool isShowCustomerComm, BettingTicketCenterExcelOption option);

        void AddNoInfoRow(BettingTicketCenterExcelOption option);

        void AddTitle(string title, BettingTicketCenterExcelOption option);

        void AssignRowData(TicketResult ticketResult, LrfTicket ticket, bool isShowCustomerComm, int licenseeId, BettingTicketCenterExcelOption option);

        void InitializeCellStyles();

        void InitializeFonts();
    }
}