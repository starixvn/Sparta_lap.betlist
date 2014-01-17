// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossData.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System.Collections.Generic;

    /// <summary>
    /// DailyRacingWinLossData class.
    /// </summary>
    public class DailyRacingWinLossData
    {
        /* dictionary key = league + currency name*/

        public IDictionary<string, List<DailyRacingWinLoss>> ContentData { get; set; }

        public IDictionary<string, DailyRacingWinLoss> ContentTotalData { get; set; }

        public IDictionary<string, DailyRacingWinLoss> CurrencyTotalData { get; set; }

        public DailyRacingWinLoss GrandTotalData { get; set; }
    }
}