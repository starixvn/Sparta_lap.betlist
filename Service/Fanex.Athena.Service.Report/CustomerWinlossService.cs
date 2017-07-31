// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    public class CustomerWinlossService : ICustWinlossService
    {
        private readonly IBalanceUpService _balanceUpService;
        private readonly ICustWinlossDataAccess _custWinlossDataAccess;

        public CustomerWinlossService()
        {
            _balanceUpService = new BalanceUpService();
            _custWinlossDataAccess = new CustomerWinlossDataAccess(_balanceUpService.BalanceUpInfo);
        }

        public SortableList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRows)
        {
            totalRows = 0;
            totalData = new CustomerWinLoss();
            return metaModel.IsValid
                       ? _custWinlossDataAccess.GetDataList(metaModel, out totalData, out totalRows)
                       : new SortableList<CustomerWinLoss>();
        }
    }
}