namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceService
    {
        IDictionary<int, string> GetSubsidiaryList(string licenseeName, string currencyIdList);

        IEnumerable<SubsidiaryPerformanceDataModel> GetData(
            SubsidiaryPerformanceMetaModel metaModel,
            out IEnumerable<SubsidiaryPerformanceEntity> totalDataList);
    }
}
