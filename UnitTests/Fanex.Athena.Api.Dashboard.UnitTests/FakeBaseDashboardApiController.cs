using Fanex.Athena.Model.Dashboard.MetaModel;

namespace Fanex.Athena.Api.Dashboard.UnitTests
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
