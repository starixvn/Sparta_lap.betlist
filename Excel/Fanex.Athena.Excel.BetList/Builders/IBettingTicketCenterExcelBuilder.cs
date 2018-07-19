using Fanex.Athena.Model.BetList.DTO;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.BetList.Builders
{
    public interface IBettingTicketCenterExcelBuilder
    {
        void CreateSheet(string sheetName);

        IWorkbook GetWorkbook();

        void AddFooter(decimal? sumStake, decimal? sumWinLoss, decimal? sumLicenseeWL);

        void AddHeader(bool isShowCustomerComm);

        void AddNoInfoRow();

        void AddTitle(string title);

        void AssignRowData(TicketResult ticketResult, LrfTicket ticket, bool isShowCustomerComm, int licenseeId);

        void InitializeCellStyles();

        void InitializeFonts();
    }
}