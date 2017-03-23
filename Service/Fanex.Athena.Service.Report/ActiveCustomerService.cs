// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveCustomerService.cs" company="">
// The licensee win loss service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class ActiveCustomerService : IActiveCustomerService
    {
        private readonly IActiveCustomerSummaryDataAccess _activeCustomerDataAccess;

        public ActiveCustomerService()
        {
            this._activeCustomerDataAccess = new ActiveCustomerSummaryDataAccess();
        }

        public SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer)
        {
            totalCustomer = 0;
            return metaModel.IsValid
                       ? this._activeCustomerDataAccess.GetActiveCustomers(metaModel, out totalCustomer)
                       : new SortableList<ActiveCustomer>();
        }
    }
}