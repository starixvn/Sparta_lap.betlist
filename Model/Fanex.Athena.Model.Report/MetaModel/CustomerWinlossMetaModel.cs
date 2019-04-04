// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class CustomerWinLossMetaModel : BaseReportMetaModel
    {
        public CustomerWinLossMetaModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            PageIndex = 1;
            PageSize = DefaultPageSize;
            OrderItem = 0;
            IsAscSort = true;
        }

        public bool IsHistory { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int OrderItem { get; set; }

        public bool IsAscSort { get; set; }

        public string Username { get; set; }
    }
}