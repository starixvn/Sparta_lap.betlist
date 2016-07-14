// -----------------------------------------------------------------------
// <copyright file="MatchWinLossMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// MatchWinLossMetaModel class.
    /// </summary>
    public class MatchWinLossMetaModel : BaseReportMetaModel
    {
        public MatchWinLossMetaModel()
        {
            Date = DateTime.Now;
        }

        public string BalanceUpDay { get; set; }

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public DateTime Date { get; set; }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}