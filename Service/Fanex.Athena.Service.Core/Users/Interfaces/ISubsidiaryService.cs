namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface ISubsidiaryService
    {
        IEnumerable<string> GetSubsidiaryNames(string licenseeName);

        IEnumerable<Subsidiary> GetSubsidiaryList(string licenseeName);

        IDictionary<int, IEnumerable<int>> GetCurrencyAgentList(string subsidiaryNames);
    }
}