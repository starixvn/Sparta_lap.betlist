namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface ISubsidiaryService
    {
        IEnumerable<string> GetSubsidiaryNames();

        IEnumerable<Subsidiary> GetSubsidiaryList();

        IDictionary<int, IEnumerable<int>> GetCurrencyAgentList(string subsidiaryNames, bool isExcludeTest);
    }
}