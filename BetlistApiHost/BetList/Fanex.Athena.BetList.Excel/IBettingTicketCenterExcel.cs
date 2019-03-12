using Fanex.Athena.BetList.Excel.Model;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Excel
{
    public interface IBettingTicketCenterExcel
    {
        IWorkbook Export(BettingTicketCenterExcelModel excelModel);
    }
}