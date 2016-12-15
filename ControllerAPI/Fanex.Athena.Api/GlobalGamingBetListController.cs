namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;

    public class GlobalGamingBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Detail(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}