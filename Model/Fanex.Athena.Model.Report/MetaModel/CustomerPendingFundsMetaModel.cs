// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    /// <summary>
    /// CustomerPendingFundsMetaModel class.
    /// </summary>
    public class CustomerPendingFundsMetaModel : BaseReportMetaModel
    {
        public CustomerPendingFundsMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
            OrderItem = 0;
            IsAscSort = true;
        }

        public int OrderItem { get; set; }

        public bool IsAscSort { get; set; }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}