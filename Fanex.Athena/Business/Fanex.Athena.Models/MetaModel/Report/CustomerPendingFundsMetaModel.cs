// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    /// <summary>
    /// CustomerPendingFundsMetaModel class.
    /// </summary>
    public class CustomerPendingFundsMetaModel : BaseReportMetaModel
    {
        public CustomerPendingFundsMetaModel()
        {
            LicenseeType = string.Empty;
            PageIndex = 1;
            PageSize = 1000;
            SortDirection = "ASC";
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