using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class PGSoftResultPopupController : BaseResultController
    {
        private readonly IIpInfoService _IpInfoService;
        private readonly IThirdPartyResult _resultApi;

        public PGSoftResultPopupController(IIpInfoService IpInfoService, IThirdPartyResult resultApi = null)
        {
            _IpInfoService = IpInfoService;
            _resultApi = resultApi ?? new ThirdPartyResult("PGSoftResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            var clientIp = string.IsNullOrEmpty(metaModel.ClientIp) ? Request.UserHostAddress : metaModel.ClientIp;

            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId,
                Location = _IpInfoService.GetIpLocation(clientIp)
            };

            return _resultApi.GetResultGET(request);
        }
    }
}