namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface ISubsidiaryService
    {
        IEnumerable<string> GetOperatorNames(string licenseeName);

        IEnumerable<Subsidiary> GetOperatorList(string licenseeName);
    }
}