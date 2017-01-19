namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public class SportsbookWalletDataAccess : ISportsbookWalletDataAccess
    {
        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return new SimpleWidgetViewModel { Value = 1570000M };
        }
    }
}
