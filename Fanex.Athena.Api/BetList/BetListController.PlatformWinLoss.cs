// -----------------------------------------------------------------------
// <copyright file="BetListController.PlatformWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        [HttpPost]
        public dynamic PlatformWinLoss(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (metaModel.CurrencyId > 0 && !currencyList.Values.Contains(metaModel.CurrencyId) &&
                (licenseeId == Licensees.KeenOcean && metaModel.CurrencyId != CurrencyType.Cny))
            {
                return null;
            }

            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_PlatformWinLoss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}