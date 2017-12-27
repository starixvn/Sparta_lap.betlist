// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalance.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    public class AgCasinoBalance : ThirdPartyWinLoss
    {
        public decimal OpeningBalance { get; set; }

        public decimal TransferIn { get; set; }

        public decimal TransferOut { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}