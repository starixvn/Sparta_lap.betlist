// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;
    using Fanex.Athena.Model.Core.DataDomain;

    /// <summary>
    /// CustomerWinLossMetaModel class.
    /// </summary>
    public class CustomerWinLossMetaModel : BaseReportMetaModel
    {
        public CustomerWinLossMetaModel()
        {
            LicenseeType = string.Empty;
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            PageIndex = 1;
            PageSize = 1000;
            SortDirection = "ASC";
            SortColumn = "ExternalId";
            DrawFilter = DrawFilterType.Normal;
        }

        public bool IsExcludedTestData { get; set; }

        public int DrawFilter { get; set; }

        public string SortDirection { get; set; }

        public string SortColumn { get; set; }

        public int PageSizeExcel
        {
            get
            {
                return int.MaxValue - 1000;
            }
        }
    }
}