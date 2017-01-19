namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public interface ITodayDepositDataAccess
    {
        SimpleWidgetViewModel GetData(DashboardBaseModel metaModel);
    }
}