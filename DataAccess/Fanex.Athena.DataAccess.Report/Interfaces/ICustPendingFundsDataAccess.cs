// -----------------------------------------------------------------------
// <copyright file="ICustPendingFundsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustPendingFundsDataAccess
    {
        Task<ReportResultModel<CustPendingFunds>> GetDataList(CustomerPendingFundsMetaModel metaModel);
    }
}