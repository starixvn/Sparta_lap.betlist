namespace Fanex.Athena.Api.BetList
{
    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;

    public partial class BetListController
    {
        [HttpPost]
        public dynamic VoidbridgeDetail(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}
