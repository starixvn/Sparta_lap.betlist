// --------------------------------------------------------------------------
// <copyright file="MicroGamingRngBetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MicroGamingRngBetListController : ApiBetListBaseController
    {
        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic Detail(BetListMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;

            const string UspKey = "Lrf_BetList_RNG_Detail";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}