namespace Fanex.Athena.Service.Report.Interfaces
{
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceService
    {
        SubsidiaryResultViewModel GetData(SubsidiaryPerformanceMetaModel metaModel);

        ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel);
    }
}