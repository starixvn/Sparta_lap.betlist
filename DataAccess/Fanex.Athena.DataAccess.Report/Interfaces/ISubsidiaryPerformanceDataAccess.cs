namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceDataAccess
    {
        IEnumerable<Operator> GetOperatorList(string licenseeName);

        ReportResultModel<SubsidiaryPerformanceEntity> GetData(SubsidiaryPerformanceMetaModel metaModel);
    }
}