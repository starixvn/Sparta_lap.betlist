// -----------------------------------------------------------------------
// <copyright file="CustPendingFunds.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    /// <summary>
    /// CustomerPendingFunds class.
    /// </summary>
    public class CustPendingFunds
    {
        public int CustId { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int BetCounts { get; set; }

        public decimal PendingFunds { get; set; }
    }
}