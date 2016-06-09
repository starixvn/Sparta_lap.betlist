// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    /// <summary>
    /// Customer balance service class.
    /// </summary>
    public class CustomerBalanceService : ICustBalanceService
    {
        private readonly ICustBalanceDataAccess _custBalanceDataAccess;

        public CustomerBalanceService()
        {
            _custBalanceDataAccess = new CustomerBalanceDataAccess();
        }

        public IList<CustBalance> GetDataList(CustomerBalanceMetaModel metaModel, ref CustBalance totalData, ref int totalRow)
        {
            return metaModel.IsValid
                       ? _custBalanceDataAccess.GetDataList(metaModel, ref totalData, ref totalRow)
                       : new List<CustBalance>();
        }
    }
}