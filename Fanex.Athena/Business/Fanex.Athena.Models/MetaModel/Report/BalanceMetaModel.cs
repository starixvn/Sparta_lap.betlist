// -----------------------------------------------------------------------
// <copyright file="BalanceMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// BalanceMetaModel class.
    /// </summary>
    public class BalanceMetaModel : BaseReportMetaModel
    {
        public BalanceMetaModel()
        {
            /*From Date = 1st day of current month, To Date = last day of current month.*/
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        public DateTime MoveBetsDay { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}