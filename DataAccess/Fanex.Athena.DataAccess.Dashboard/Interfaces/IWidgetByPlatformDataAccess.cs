namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public interface IWidgetByPlatformDataAccess
    {
        WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel);
    }
}