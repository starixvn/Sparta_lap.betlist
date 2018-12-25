namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceDataAccess
    {
        IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}