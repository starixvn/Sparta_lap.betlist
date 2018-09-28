namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceDataAccess
    {
        IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel metaModel);

        ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel);
    }
}