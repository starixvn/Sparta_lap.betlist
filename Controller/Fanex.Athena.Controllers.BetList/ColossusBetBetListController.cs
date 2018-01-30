// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Web.Mvc;
    using System.Web.UI;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Newtonsoft.Json.Linq;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;
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

        public ColossusBetBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IWebClientService webClientService,
            IColossusBetBetListDataAccess colossusBetBetListDataAccess)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper,
            authorizationService,
            productService,
            betListService,
            balanceUpDataAccess,
            cacheRefAdapter)
        {
            _colossusBetBetListDataAccess = colossusBetBetListDataAccess;
            _webClientService = webClientService;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
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
            var json = string.Empty;
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