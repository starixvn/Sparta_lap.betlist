// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveCustomerService.cs" company="Nexcel">
// The licensee win loss service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The active customer service.
    /// </summary>
    public class ActiveCustomerService : IActiveCustomerService
    {
        #region Constants and Fields
        
        private readonly IActiveCustomerSummaryDataAccess _activeCustomerDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActiveCustomerService" /> class.
        /// </summary>
        public ActiveCustomerService()
        {
            this._activeCustomerDataAccess = new ActiveCustomerSummaryDataAccess();
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        /// <summary>
        ///     The get active customers.
        /// </summary>
        /// <param name="metaModel">
        ///     The metaModel.
        /// </param>
        /// <param name="totalCustomer">The total customer value.</param>
        /// <returns>The active customer list.</returns>
        public SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer)
        {
            totalCustomer = 0;
            return metaModel.IsValid
                       ? this._activeCustomerDataAccess.GetActiveCustomers(metaModel, out totalCustomer)
                       : new SortableList<ActiveCustomer>();
        }

        #endregion Public Methods and Operators
    }
}