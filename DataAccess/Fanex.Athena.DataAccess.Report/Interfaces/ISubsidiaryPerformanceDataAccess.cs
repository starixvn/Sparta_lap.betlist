namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceDataAccess
    {
        IDictionary<int, string> GetSubsidiaryList(string licenseeName);

        IEnumerable<SubsidiaryPerformanceEntity> GetData(SubsidiaryPerformanceMetaModel metaModel, out IEnumerable<SubsidiaryPerformanceEntity> totalDataList);
    }
}