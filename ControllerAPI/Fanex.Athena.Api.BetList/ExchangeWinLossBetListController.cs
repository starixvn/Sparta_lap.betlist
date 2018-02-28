namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.BetList;
    using Service.BetList.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossBetListController : BaseApiController
    {
        private readonly IExchangeWinLossBetListService _service;

        public ExchangeWinLossBetListController()
        {
            _service = new ExchangeWinLossBetListService();
        }

        public ExchangeWinLossBetListController(IExchangeWinLossBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            ////metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var data = _service.GetBetListData(metaModel);

            return new
            {
                MainData = GroupMainData(data),
                GrandTotalData = GetGrandTotalData(data)
            };
        }

        private dynamic GroupMainData(IEnumerable<ExchangeTicket> data)
        {
            return data.GroupBy(x => x.Market).Select(x => new
            {
                Market = x.Key,
                WinLoss = x.Sum(d => d.WinLoss),
                WlComm = x.Sum(d => d.WlComm),
                LicWl = x.Sum(d => d.LicWl),
                LicWlComm = x.Sum(d => d.LicWlComm),
                Details = x.Select(d => new { d.BetId, d.Selection, d.Type, d.BetPlaced, d.OddsAvg, d.Stake, d.WinLoss }).ToList()
            }).ToList();
        }

        private dynamic GetGrandTotalData(IEnumerable<ExchangeTicket> data)
        {
            return new
            {
                TotalWl = data.Sum(x => x.WinLoss + x.WlComm),
                TotalLicWl = data.Sum(x => x.LicWl + x.LicWlComm)
            };
        }
    }
}