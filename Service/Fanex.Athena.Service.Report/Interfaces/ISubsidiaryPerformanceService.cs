namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.DTO;

    public interface ISubsidiaryPerformanceService
    {
        IEnumerable<SubsidiaryPerformance> GetData(SubsidiaryPerformanceMetaModel subLicenseePerformanceMetaModel);
    }
}
