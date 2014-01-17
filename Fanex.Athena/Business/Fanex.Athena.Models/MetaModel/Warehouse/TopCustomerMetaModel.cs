// -----------------------------------------------------------------------
// <copyright file="TopCustomerMetaModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Warehouse
{
    using System;

    /// <summary>
    /// TopCustomerMetaModel class.
    /// </summary>
    public class TopCustomerMetaModel : BaseWarehouseMetaModel
    {
        public TopCustomerMetaModel()
        {
            ActiveMin = -1;
            SortingId = 0;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            BetCountMin = 0;
            WinLossTotalMin = 0;
            TopRow = 10;
        }

        /// <summary>
        /// Gets or sets SortingId.
        /// </summary>
        /// <value>1:Day,3:Month,-10:Currency,0:bet type,4:League.</value>
        public int SortingId { get; set; }

        /// <summary>
        /// Gets or sets ExternalId.
        /// </summary>
        /// <value>1:normal,2:No draws,3:No draws for turnover only.</value>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets ActiveMin.
        /// </summary>
        /// <value> Values: -1:All, 14: 2 weeks, 30: 1 month, 90: 3 months, 180: 6 months.</value>
        public int ActiveMin { get; set; }

        public decimal WinLossTotalMin { get; set; }

        public int BetCountMin { get; set; }

        /// <summary>
        /// Gets or sets SportId.
        /// </summary>
        /// <value>0: All, -1: Other sports.</value>
        public int SportId { get; set; }

        public int TopRow { get; set; }
    }
}