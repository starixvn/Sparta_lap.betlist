namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class PendingFundDataAccess : ISimpleWidgetDataAccess
    {
        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return new SimpleWidgetViewModel { Value = 250000M };
        }
    }
}