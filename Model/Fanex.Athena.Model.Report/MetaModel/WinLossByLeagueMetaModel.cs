// -----------------------------------------------------------------------
// <copyright file="WinLossByLeagueMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// TurnoverMetaModel class.
    /// </summary>
    public class WinLossByLeagueMetaModel : BaseReportMetaModel
    {
        public WinLossByLeagueMetaModel()
        {                           
        }

        public int CustomerId { get; set; }

        public string ReportTitle { get; set; }

        /// <summary>
        /// Gets or sets level.
        /// Values:0,1,2.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        public int League { get; set; }

        public string LeagueName { get; set; }

        public int Match { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public decimal Odds { get; set; }

        public string Sports { get; set; }

        public byte ReportBy { get; set; }

        public bool ShowWin { get; set; }

        public bool ShowLoss { get; set; }

        public bool ShowDraw { get; set; }        

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}