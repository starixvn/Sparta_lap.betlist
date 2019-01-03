namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceService
    {
        SubsidiaryResultViewModel GetData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}