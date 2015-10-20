// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        [HttpPost]
        public dynamic AGCasinoDetail(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            const string UspKey = "Lrf_BetList_LC_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}
