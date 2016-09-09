// -----------------------------------------------------------------------
// <copyright file="BalanceMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Extension.Utility;

    /// <summary>
    /// BalanceMetaModel class.
    /// </summary>
    public class BalanceMetaModel : BaseReportMetaModel
    {
        public BalanceMetaModel()
            : this(new AppSettings())
        {
        }

        public BalanceMetaModel(IAppSettings appSetting)
            : base(appSetting)
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