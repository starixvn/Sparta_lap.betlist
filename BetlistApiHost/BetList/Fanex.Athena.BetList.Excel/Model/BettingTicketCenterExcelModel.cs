using Fanex.Athena.BetList.Model.DTO;

namespace Fanex.Athena.BetList.Excel.Model
{
    public class BettingTicketCenterExcelModel
    {
        public TicketResult Data { get; set; }

        public bool CustomerCommVisible { get; set; }

        public int LicenseeId { get; set; }

        public string Title { get; set; }

        public BettingTicketCenterExcelOption Option { get; set; }
    }
}
