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
            DrawFilter = DrawFilterType.Normal;
            OrderItem = 0;
            IsAscSort = true;
        }

        public bool IsHistory { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int DrawFilter { get; set; }

        public int OrderItem { get; set; }

        public bool IsAscSort { get; set; }

        public string Username { get; set; }
    }
}