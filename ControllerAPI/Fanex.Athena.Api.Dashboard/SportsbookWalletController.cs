namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class SportsbookWalletController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetService _sportsbookWalletService;

        public SportsbookWalletController() : this(null)
        {
        }

        public SportsbookWalletController(ISimpleWidgetService sportsbookWalletService)
        {
            _sportsbookWalletService = sportsbookWalletService ?? new SportsbookWalletService();
        }

        public SimpleWidgetViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            return _sportsbookWalletService.GetData(metaModel);
        }
    }
}