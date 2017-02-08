namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class PendingFundDataAccess : ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            return 250000M;
        }
    }
}