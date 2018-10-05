// -----------------------------------------------------------------------
// <copyright file="BaseReportMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Model.Core.MetaModel;

    public class BaseReportMetaModel : BaseMetaModel
    {
        public BaseReportMetaModel()
        {
            ProductOptions = new List<ProductOption>();
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }

        public string ProductCodeList { get; set; } = string.Empty;

        public bool IsExportExcel { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IList<ProductOption> ProductOptions
        {
            get;
            set;
        }

        public new bool IsConvert
        {
            get
            {
                return SystemConstants.BaseCurrencyList.Any(t => t.Equals(BaseCurrency, StringComparison.OrdinalIgnoreCase));
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return FromDate <= ToDate
                    && ProductOptions.Any(p => p.Status);
            }
        }
    }
}