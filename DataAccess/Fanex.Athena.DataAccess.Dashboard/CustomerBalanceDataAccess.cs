namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public class CustomerBalanceDataAccess : ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            return 1570000M;
        }
    }
}
