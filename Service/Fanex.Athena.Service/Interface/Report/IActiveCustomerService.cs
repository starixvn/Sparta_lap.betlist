// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActiveCustomerService.cs" company="Nexcel">
// The active customer service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     The active customer service interface.
    /// </summary>
    public interface IActiveCustomerService
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get summary win-loss table.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="totalCustomer">Total customer value.</param>
        /// <returns>The data list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer);

        #endregion Public Methods and Operators
    }
}