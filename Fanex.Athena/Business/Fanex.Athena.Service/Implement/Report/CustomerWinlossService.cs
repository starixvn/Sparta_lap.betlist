// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
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

        public IList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, ref CustomerWinLoss totalData, ref int totalRows)
        {
            return metaModel.IsValid
                       ? _custWinlossDataAccess.GetDataList(metaModel, ref totalData, ref totalRows)
                       : new List<CustomerWinLoss>();
        }
    }
}