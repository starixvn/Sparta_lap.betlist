// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossData.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// DailyRacingWinLossData class.
    /// </summary>
    public class DailyRacingWinLossData
    {
        /* dictionary key = league + currency name*/

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Reviewed. Suppression is OK here.")]
        public IDictionary<string, List<DailyRacingWinLoss>> ContentData { get; set; }

        public IDictionary<string, DailyRacingWinLoss> ContentTotalData { get; set; }

        public IDictionary<string, DailyRacingWinLoss> CurrencyTotalData { get; set; }

        public DailyRacingWinLoss GrandTotalData { get; set; }
    }
}