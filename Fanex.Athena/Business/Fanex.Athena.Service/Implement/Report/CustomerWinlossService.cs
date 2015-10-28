// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// Customer win-loss service class.
    /// </summary>
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