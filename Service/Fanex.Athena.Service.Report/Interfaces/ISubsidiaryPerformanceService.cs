namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.DTO.SubsidiaryPerformance;

    public interface ISubsidiaryPerformanceService
    {
        IEnumerable<SubsidiaryPerformanceEntity> GetData(SubsidiaryPerformanceMetaModel subLicenseePerformanceMetaModel);
    }
}
