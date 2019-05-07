namespace Fanex.Athena.Module.Dashboard.Data.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Module.Dashboard.MetaModel;
    using Fanex.Athena.Module.Dashboard.ViewModel;

    public interface ILicenseeDataByPlatformDataAccess
    {
        Task<WidgetByPlatformViewModel> GetWinLoss(DashboardBaseModel metaModel);

        Task<WidgetByPlatformViewModel> GetTurnover(DashboardBaseModel metaModel);
    }
}