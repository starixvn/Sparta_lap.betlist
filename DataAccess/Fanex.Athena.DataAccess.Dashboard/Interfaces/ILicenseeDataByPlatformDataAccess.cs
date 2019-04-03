namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using System.Threading.Tasks;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public interface ILicenseeDataByPlatformDataAccess
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(DashboardBaseModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(DashboardBaseModel metaModel);
    }
}