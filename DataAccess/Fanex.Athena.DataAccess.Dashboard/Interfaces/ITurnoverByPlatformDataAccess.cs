namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Dashboard.DTO;

    public interface ITurnoverByPlatformDataAccess
    {
        IList<Platform> GetData();
    }
}