// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// Customer pending fund service class.
    /// </summary>
    public class CustomerPendingFundsService : ICustPendingFundsService
    {
        private readonly ICustPendingFundsDataAccess _custPendingFundsDataAccess;

        public CustomerPendingFundsService()
        {
            _custPendingFundsDataAccess = new CustomerPendingFundsDataAccess();
        }

        public SortableList<CustPendingFunds> GetDataList(CustomerPendingFundsMetaModel metaModel, ref CustPendingFunds totalData, ref int totalRows)
        {
            return metaModel.IsValid
                       ? _custPendingFundsDataAccess.GetDataList(metaModel, ref totalData, ref totalRows)
                       : new SortableList<CustPendingFunds>();
        }
    }
}