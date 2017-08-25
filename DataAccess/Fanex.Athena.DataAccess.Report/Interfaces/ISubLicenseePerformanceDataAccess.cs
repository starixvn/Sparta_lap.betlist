namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.DTO;

    public interface ISubLicenseePerformanceDataAccess
    {
        IEnumerable<SubLicenseePerformance> GetData(SubLicenseePerformanceMetaModel metaModel);
    }
}