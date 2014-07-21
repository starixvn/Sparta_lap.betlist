// -----------------------------------------------------------------------
// <copyright file="WarehouseReportBy.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Warehouse
{
    /// <summary>
    /// Warehouse report by filter.
    /// </summary>
    public enum WarehouseReportBy
    {
        /// <summary>
        /// None, value = 0.
        /// </summary>
        None = 0,

        /// <summary>
        /// Report by day.
        /// </summary>
        Day = 1,

        /// <summary>
        /// Report by month.
        /// </summary>
        Month = 2,

        /// <summary>
        /// Report by currency.
        /// </summary>
        Currency = 3,

        /// <summary>
        /// Report by bet type.
        /// </summary>
        Bettype = 4,

        /// <summary>
        /// Report by league.
        /// </summary>
        League = 5
    }
}
