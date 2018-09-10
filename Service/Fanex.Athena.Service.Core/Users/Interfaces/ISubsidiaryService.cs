namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface ISubsidiaryService
    {
        IEnumerable<string> GetSubsidiaryNames(string licenseeName);

        IEnumerable<Subsidiary> GetOperatorList(string licenseeName);
    }
}