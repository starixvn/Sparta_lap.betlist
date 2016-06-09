// -----------------------------------------------------------------------
// <copyright file="LiveReportBetListMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    /// <summary>
    /// LiveReportBetListMetaModel class.
    /// </summary>
    public class LiveReportBetListMetaModel : BetListMetaModel
    {
        public decimal SumWinlost { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public int TotalRows { get; set; }
    }
}