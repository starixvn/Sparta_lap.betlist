namespace Fanex.Athena.Service.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Model.Dashboard.MetaModel;

    public interface ITurnoverByPlatformService
    {
        TurnoverByPlatformViewModel GetData(DashboardBaseModel metaModel);
    }
}