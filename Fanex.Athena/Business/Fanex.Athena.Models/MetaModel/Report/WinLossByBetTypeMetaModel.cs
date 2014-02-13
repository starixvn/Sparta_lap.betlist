// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// WinLossByBetTypeMetaModel class.
    /// </summary>
    public class WinLossByBetTypeMetaModel : BaseReportMetaModel
    {
        public WinLossByBetTypeMetaModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            SortDirection = "ASC";
            SortColumn = "BetTypeName";
        }

        public string SortDirection { get; set; }

        public string SortColumn { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}