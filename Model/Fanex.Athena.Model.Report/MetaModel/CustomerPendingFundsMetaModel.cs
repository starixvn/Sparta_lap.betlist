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
            PageSize = 1000;
            SortDirection = "asc";
            SortColumn = "ExternalId";
        }

        public string SortDirection { get; set; }

        public string SortColumn { get; set; }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}