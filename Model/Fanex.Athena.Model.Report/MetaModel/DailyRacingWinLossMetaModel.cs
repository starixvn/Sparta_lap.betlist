// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// DailyRacingWinLossMetaModel class.
    /// </summary>
    public class DailyRacingWinLossMetaModel : BaseReportMetaModel
    {
        public DailyRacingWinLossMetaModel()
        {
            CurrencyId = 0;
            Date = DateTime.Now;
        }

        public DateTime Date { get; set; }

        public string BalanceUpDay { get; set; }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}