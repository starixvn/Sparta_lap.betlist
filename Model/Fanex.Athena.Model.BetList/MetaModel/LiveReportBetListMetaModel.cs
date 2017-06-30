// -----------------------------------------------------------------------
// <copyright file="LiveReportBetListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class LiveReportBetListMetaModel : BetListMetaModel
    {
        public int SportType { get; set; }

        public decimal SumWinlost { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public int TotalRows { get; set; }
    }
}