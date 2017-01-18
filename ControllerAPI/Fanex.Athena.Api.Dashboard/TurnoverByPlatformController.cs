namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    public class TurnoverByPlatformController : ApiController
    {
        private readonly ITurnoverByPlatformService _turnoverByPlatformService;

        public TurnoverByPlatformController() : this(null)
        {
        }

        public TurnoverByPlatformController(ITurnoverByPlatformService turnoverByPlatformService)
        {
            this._turnoverByPlatformService = turnoverByPlatformService ?? new TurnoverByPlatformService();
        }

        [HttpGet]
        public TurnoverByPlatformViewModel GetData()
        {
            return _turnoverByPlatformService.GetData();
        }
    }
}