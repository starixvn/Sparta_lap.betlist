using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Extension.Utility.Interfaces;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Logging;
using Newtonsoft.Json.Linq;

namespace Fanex.Athena.BetList.Controller
{
    public class ColossusBetBetListController : BaseController
    {
        private readonly IAppSettings _appSettings;

        private readonly ILogger _logger;

        private readonly IColossusBetBetListDataAccess _colossusBetBetListDataAccess;

        private readonly IWebClientService _webClientService;

        public ColossusBetBetListController(
            IAppSettings appSettings,
            ILogger logger,
            IWebClientService webClientService,
            IColossusBetBetListDataAccess colossusBetBetListDataAccess)
        {
            _appSettings = appSettings;
            _logger = logger;
            _colossusBetBetListDataAccess = colossusBetBetListDataAccess;
            _webClientService = webClientService;
        }

        [HttpPost]
        public JsonResult Detail(ColossusBetDetailMetaModel metaModel)
        {
            var viewModel = _colossusBetBetListDataAccess.GetData(metaModel);

            var iframeUrl = GetIframeUrl(metaModel.UserId, metaModel.BetCheck, metaModel.CustId);
            viewModel.IframeUrl = iframeUrl;
            return Json(viewModel);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        private string GetIframeUrl(int userId, long betCheck, int memberId)
        {
            var token = GetToken(userId, memberId);
            var siteId = _colossusBetBetListDataAccess.GetSiteId(memberId);
            var url = _appSettings.GetValue("CBIframeUrlSiteId" + siteId.ToString());
            var colossusBetSiteId = _appSettings.GetValue("ColossusBetSiteId");
            return string.Format(url, betCheck, colossusBetSiteId, token);
        }

        private string GetToken(int userId, int memberId)
        {
            var tokenApiUrl = _appSettings.GetValue("ColossusBetTokenAPI");
            tokenApiUrl = string.Format(tokenApiUrl, userId, memberId);
            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(tokenApiUrl);
            }
            catch (Exception exception)
            {
                _logger.Error("ColossusBet Get Token Error - ApiUrl: " + tokenApiUrl, exception);
                return string.Empty;
            }

            var jobject = JObject.Parse(json);
            var token = (string)jobject.SelectToken("data.authorization");
            if (token == null)
            {
                var message = $"Errors from Colossus Bets API (memberid: {memberId}).\r\n" + jobject;
                _logger.Error(message);
                return string.Empty;
            }

            return token;
        }
    }
}
