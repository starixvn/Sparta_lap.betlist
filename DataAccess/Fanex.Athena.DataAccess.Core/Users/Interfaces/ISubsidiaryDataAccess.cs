namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Model.Report.DTO;

    public interface ISubsidiaryDataAccess
    {
        IEnumerable<Subsidiary> GetSubsidiaryList(string licenseeName);
    }
}