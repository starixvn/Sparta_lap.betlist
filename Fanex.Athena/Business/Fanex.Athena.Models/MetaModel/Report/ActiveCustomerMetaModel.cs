// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// ActiveCustomerMetaModel class.
    /// </summary>
    public class ActiveCustomerMetaModel : BaseReportMetaModel
    {
        public byte ReportBy { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}