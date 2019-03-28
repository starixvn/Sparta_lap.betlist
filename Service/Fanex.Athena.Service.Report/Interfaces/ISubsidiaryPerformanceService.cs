namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceService
    {
        Task<SubsidiaryResultViewModel> GetData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<SubsidiaryPerformanceDetail>> GetDetailData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}