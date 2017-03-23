// -----------------------------------------------------------------------
// <copyright file="WinLossByProductMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Extension.Utility;

    public class WinLossByProductMetaModel : BaseReportMetaModel
    {
        public WinLossByProductMetaModel()
            : this(new AppSettings())
        {
        }

        public WinLossByProductMetaModel(IAppSettings appSetting)
            : base(appSetting)
        {
        }

        #region Members

        public int DrillDownCurrencyId { get; set; }

        public string DrillDownCurrencyName { get; set; }

        public byte ReportBy { get; set; }

        public int DrawFilter { get; set; }

        public bool IsExcludedTestData { get; set; }

        /// <summary>
        /// Gets or sets level.
        /// Values:0,1,2.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets identity of customer .
        /// Only use for level 2.
        /// </summary>
        /// <value>The identity of customer.</value>
        public int CustomerId { get; set; }

        public int MemberId { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public string Username { get; set; }

        #endregion Members
    }
}