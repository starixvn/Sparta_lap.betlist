// -----------------------------------------------------------------------
// <copyright file="WinLossMetaModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Warehouse
{
    using System;

    /// <summary>
    /// WinLossMetaModel class.
    /// </summary>
    public class WinLossMetaModel : BaseWarehouseMetaModel
    {
        public WinLossMetaModel()
        {
            DrawFilter = 2;
            ReportBy = 1;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets  type of reports.
        /// 1:Day,3:Month,-10:Currency,0:bet type,4:League.
        /// </summary>
        /// <value>The report type.</value>
        public int ReportBy { get; set; }

        /// <summary>
        /// Gets or sets draw filter.
        /// 1:normal,2:No draws,3:No draws for turnover only.
        /// </summary>
        /// <value>The draw filter.</value>
        public int DrawFilter { get; set; }
    }
}