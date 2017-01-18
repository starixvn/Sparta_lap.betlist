using Fanex.Athena.DataAccess.Dashboard.Interfaces;
using Fanex.Athena.Model.Dashboard.MetaModel;
using Fanex.Athena.Model.Dashboard.ViewModel;

namespace Fanex.Athena.DataAccess.Dashboard
{
    public class TodayDepositDataAccess : ITodayDepositDataAccess
    {
        public TodayDepositViewModel GetData(DashboardBaseModel metaModel)
        {
            return new TodayDepositViewModel { Value = 2500000M };
        }
    }
}
