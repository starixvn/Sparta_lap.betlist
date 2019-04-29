using Fanex.Athena.Module.Dashboard.Api;
using Fanex.Athena.Module.Dashboard.MetaModel;

namespace Fanex.Athena.Module.Dashboard.UnitTests.Api
{
    internal class FakeBaseDashboardApiController : BaseDashboardApiController
    {
        internal new int LicenseeId => base.LicenseeId;

        internal new int UserId => base.UserId;

        internal new string CurrencyIDList => base.CurrencyIDList;

        internal new void AssignAuthProperties(DashboardBaseModel model)
        {
            base.AssignAuthProperties(model);
        }
    }
}
