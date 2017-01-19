using Fanex.Athena.DataAccess.Dashboard.Interfaces;
using Fanex.Athena.Model.Dashboard.MetaModel;
using Fanex.Athena.Model.Dashboard.ViewModel;

namespace Fanex.Athena.DataAccess.Dashboard
{
    public class TodayDepositDataAccess : ISimpleWidgetDataAccess
    {
        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return new SimpleWidgetViewModel { Value = 2500000M };
        }
    }
}
