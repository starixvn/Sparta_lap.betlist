using System.Web.Http;

namespace Fanex.Athena.Api.Dashboard
{
    public class TurnoverByPlatformController : ApiController
    {
        [HttpGet]
        public TurnoverByPlatformController Get()
        {
            return new TurnoverByPlatformController();
        }
    }
}
