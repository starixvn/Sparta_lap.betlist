// -----------------------------------------------------------------------
// <copyright file="RacingCommission.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    /// <summary>
    /// RacingCommission class.
    /// </summary>
    public class RacingCommission
    {
        public int CustID { get; set; }

        public string ExternalID { get; set; }

        public string SystemID { get; set; }

        public decimal ToteWin { get; set; }

        public decimal TotePlace { get; set; }

        public decimal ToteWinPlace { get; set; }

        public decimal ToteTotal { get; set; }

        public decimal CustomerCommission { get; set; }
    }
}