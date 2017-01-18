namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Dashboard.DTO;
    using Model.Dashboard.MetaModel;

    public interface ITurnoverByPlatformDataAccess
    {
        IList<Platform> GetData(DashboardBaseModel metaModel);
    }
}