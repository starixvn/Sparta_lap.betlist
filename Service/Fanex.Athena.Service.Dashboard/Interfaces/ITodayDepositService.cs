namespace Fanex.Athena.Service.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public interface ITodayDepositService
    {
        SimpleWidgetViewModel GetData(DashboardBaseModel metaModel);
    }
}
