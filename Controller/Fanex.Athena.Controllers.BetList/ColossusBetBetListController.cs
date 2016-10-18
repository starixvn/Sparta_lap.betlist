// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System;

    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Newtonsoft.Json.Linq;
    using Service.BetList;
    using Service.BetList.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportColossusBetsWinLoss)]
    public class ColossusBetBetListController : BetListBaseController
    {
        private readonly IColossusBetBetListService _colossusBetBetListService;

        private readonly IWebClientService _webClientService;

        public ColossusBetBetListController()
        {
            _colossusBetBetListService = new ColossusBetBetListService();
            _webClientService = new WebClientService();
        }

        [HttpGet]
        public ActionResult Detail(ColossusBetDetailMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var viewModel = _colossusBetBetListService.GetData(metaModel);

            var iframeUrl = GetIframeUrl(metaModel.BetCheck, metaModel.CustId);
            viewModel.IframeUrl = iframeUrl;
            return this.View("~/Views/BetList/ColossusBetDetail/Main.cshtml", viewModel);
        }

        private string GetIframeUrl(long betCheck, int memberId)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var token = GetToken(memberId);
            var siteId = _colossusBetBetListService.GetSiteId(memberId);
            var url = AppSettings.GetValue("CBIframeUrlSiteId" + siteId.ToString());
            var colossusBetSiteId = AppSettings.GetValue("ColossusBetSiteId");
            return string.Format(url, betCheck, colossusBetSiteId, token);
        }

        private string GetToken(int memberId)
        {
            var tokenApiUrl = AppSettings.GetValue("ColossusBetTokenAPI");
            tokenApiUrl = string.Format(tokenApiUrl, SessionService.UserSession.UserId, memberId);
            string json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(tokenApiUrl);
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception, "ApiUrl: " + tokenApiUrl);
                return string.Empty;
            }

            var jobject = JObject.Parse(json);
            var token = (string)jobject.SelectToken("data.authorization");
            if (token == null)
            {
                var message = $"Errors from Colossus Bets API (memberid: {memberId}).\r\n" + jobject;
                LogHelper.WriteError(message);
                return string.Empty;
            }

            return token;
        }
    }
}