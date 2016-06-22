// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// CustomerBalanceMetaModel class.
    /// </summary>
    public class CustomerBalanceMetaModel : BaseReportMetaModel
    {
        public CustomerBalanceMetaModel()
        {
            FromDate = ToDate = DateTime.Today;
            IsExcludeTest = true;
            TopCount = 0;
            OrderItem = 1;
            PageIndex = 1;
            PageSize = 1000;
            LicenseeType = string.Empty;
        }

        public int TopCount { get; set; }

        public short OrderItem { get; set; }

        public bool IsExcludeTest { get; set; }
    }
}