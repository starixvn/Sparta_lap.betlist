// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class CustomerBalanceMetaModel : BaseReportMetaModel
    {
        public CustomerBalanceMetaModel()
        {
            FromDate = ToDate = DateTime.Today;
            IsExcludeTest = true;
            PageIndex = 1;
            PageSize = 1000;
            OrderItem = 1;
        }

        public bool IsExcludeTest { get; set; }

        public int TopCount { get; set; }

        public short OrderItem { get; set; }
    }
}