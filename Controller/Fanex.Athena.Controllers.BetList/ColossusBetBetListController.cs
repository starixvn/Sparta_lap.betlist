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
    using System.Web.UI;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
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
        private readonly IColossusBetBetListDataAccess _colossusBetBetListDataAccess;

        private readonly IWebClientService _webClientService;

        public ColossusBetBetListController()
        {
            _colossusBetBetListDataAccess = new ColossusBetBetListDataAccess();
            _webClientService = new WebClientService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Detail(ColossusBetDetailMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            var viewModel = _colossusBetBetListDataAccess.GetData(metaModel);

            var iframeUrl = GetIframeUrl(metaModel.BetCheck, metaModel.CustId);
            viewModel.IframeUrl = iframeUrl;
            return this.View("~/Views/BetList/ColossusBetDetail/Main.cshtml", viewModel);
        }

        private string GetIframeUrl(long betCheck, int memberId)
        {
            var token = GetToken(memberId);
            var siteId = _colossusBetBetListDataAccess.GetSiteId(memberId);
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