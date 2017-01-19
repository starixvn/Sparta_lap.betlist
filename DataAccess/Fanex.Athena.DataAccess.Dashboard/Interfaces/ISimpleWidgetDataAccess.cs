namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public interface ISimpleWidgetDataAccess
    {
        SimpleWidgetViewModel GetData(DashboardBaseModel metaModel);
    }
}