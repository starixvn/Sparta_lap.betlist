namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public interface ILicenseeDataByPlatformDataAccess
    {
        WidgetByPlatformViewModel GetWinLoss(DashboardBaseModel metaModel);

        WidgetByPlatformViewModel GetTurnover(DashboardBaseModel metaModel);
    }
}