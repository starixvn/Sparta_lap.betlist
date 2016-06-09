// -----------------------------------------------------------------------
// <copyright file="AGCasinoBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AGCasinoBetListController : ApiBetListBaseController
    {
        [HttpPost]
        public dynamic Detail(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            const string UspKey = "Lrf_BetList_LC_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}
