// -----------------------------------------------------------------------
// <copyright file="ICustWinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustWinlossDataAccess
    {
        Task<ReportResultModel<CustomerWinLoss>> GetDataList(CustomerWinLossMetaModel metaModel);
    }
}