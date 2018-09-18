using Fanex.Athena.Excel.BetList.Model;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.BetList
{
    public interface IBettingTicketCenterExcel
    {
        IWorkbook Export(BettingTicketCenterExcelModel excelModel);
    }
}