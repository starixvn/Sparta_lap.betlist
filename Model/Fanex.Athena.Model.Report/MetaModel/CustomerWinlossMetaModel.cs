// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;

    public class CustomerWinLossMetaModel : BaseReportMetaModel
    {
        public CustomerWinLossMetaModel()
            : this(new AppSettings())
        {
        }

        public CustomerWinLossMetaModel(IAppSettings appSetting)
            : base(appSetting)
        {
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