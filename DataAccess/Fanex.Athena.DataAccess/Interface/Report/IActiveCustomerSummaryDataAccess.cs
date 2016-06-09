// -----------------------------------------------------------------------
// <copyright file="IActiveCustomerSummaryDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     The active customer data access.
    /// </summary>
    public interface IActiveCustomerSummaryDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer);

        #endregion Public Methods and Operators
    }
}