namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class SportsbookWalletController : ApiController
    {
        private readonly ISimpleWidgetService _sportsbookWalletService;

        public SportsbookWalletController() : this(null)
        {
        }

        public SportsbookWalletController(ISimpleWidgetService sportsbookWalletService)
        {
            _sportsbookWalletService = sportsbookWalletService ?? new SportsbookWalletService();
        }

        [HttpPost]
        public SimpleWidgetViewModel Index(DashboardBaseModel metaModel)
        {
            return _sportsbookWalletService.GetData(metaModel);
        }
    }
}