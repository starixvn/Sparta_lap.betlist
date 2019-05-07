namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface ISpecialLicenseeWinLossDataAccess
    {
        Task<ReportResultModel<LicenseeWinLoss>> GetData(SpecialLicenseeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}