// -----------------------------------------------------------------------
// <copyright file="IActiveCustomerSummaryDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface IActiveCustomerSummaryDataAccess
    {
        Task<ActiveCustomerSummaryResultModel> GetActiveCustomers(ActiveCustomerMetaModel metaModel);
    }
}