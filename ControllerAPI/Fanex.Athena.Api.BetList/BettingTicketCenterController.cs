namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Constant.DataDomain;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.BetList.Extensions;
    using Service.BetList.Helpers;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterController : BaseApiController
    {
        private readonly IBettingTicketCenterDataAccess _dataAccess;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly ITicketHelper _ticketHelper;

        public BettingTicketCenterController()
        {
            _dataAccess = new BettingTicketCenterDataAccess();
            _betlistHtmlBuilder = new BetListHTMLBuilder();
            _ticketHelper = new TicketHelper();
            _cacheRefAdapter = new CacheRefAdapter();
        }

#pragma warning disable S107 // Methods should not have too many parameters

        public BettingTicketCenterController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IBettingTicketCenterDataAccess dataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(sessionService, productService, langService)
        {
            _dataAccess = dataAccess;
            _betlistHtmlBuilder = betlistHtmlBuilder;
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
        }

        [HttpPost]
        public dynamic GetByMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            var showPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForMultipleCustomers(metaModel);

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = _betlistHtmlBuilder.BuildCustomerWL(new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm }, showPlayerComm),
                    SumLicenseeWl = (ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat),
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        [HttpPost]
        public dynamic GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid && !IsValidCurrencyId(metaModel.CurrencyId))
            {
                return null;
            }

            var showPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = _dataAccess.GetBetListForSingleCustomer(metaModel);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = true,
                    ShowSumStake = true,
                    ShowLicenseeWl = true,
                    ShowWinLoss = true,
                    CustomerCommVisible = showPlayerComm,
                    Title = GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = _betlistHtmlBuilder.BuildCustomerWL(new LrfTicket { Winlost = ticketResult.SumWinLoss, PlayerComm = ticketResult.SumPlayerComm }, showPlayerComm),
                    SumLicenseeWL = (ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat),
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        [HttpPost]
        public dynamic GetByRefno(RefNoBetListMetaModel metaModel)
        {
            TicketResult ticketResult;
            if (!metaModel.IsValid)
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                ticketResult = _dataAccess.GetBetListByRefno(metaModel);
            }

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = false,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            TicketResult ticketResult;
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                ticketResult = _dataAccess.GetRunningBetList(metaModel);
            }

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = true,
                    ShowSumStake = true,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        protected internal virtual BettingTicketCenterExcel CreateExcel(BettingTicketCenterExcelModel excelModel)
        {
            return new BettingTicketCenterExcel(excelModel);
        }

        private static string GetTitle(BaseBettingTicketMetaModel metaModel)
        {
            var date = metaModel.FromDate == DateTime.MinValue
                ? string.Empty
                : ", " + metaModel.FromDate.ToString(FormatKey.ShortDateFormat) + "-->" + metaModel.ToDate.ToString(FormatKey.ShortDateFormat);
            var currency = string.IsNullOrWhiteSpace(metaModel.CurrencyName) ? string.Empty : ", " + ViewResources.Currency + ": " + metaModel.CurrencyName;

            return ViewResources.BettingTicketCenter
                    + date
                    + currency
                    + ", " + ViewResources.BaseCurrency + ": " + metaModel.BaseCurrency.ToUpperInvariant();
        }

        private bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == BaseCurrencyType.All || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }

        private HttpResponseMessage Export(BettingTicketCenterExcelModel excelModel)
        {
            var excel = CreateExcel(excelModel);
            var workbook = excel.Export();
            return ExcelStream(workbook, string.Empty);
        }

        private List<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel)
        {
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var showPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
            var ticketTextList = new List<TicketText>();

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
                {
                    var baseCurrency = metaModel.IsConvertAll ? metaModel.BaseCurrency : ticket.Currency;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, ticket.CustId);
                }

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = _betlistHtmlBuilder.BuildTrans(ticket),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = _betlistHtmlBuilder.BuildOdds(ticket, ticketResult.TicketDataList, _cacheRefAdapter.GetOddsTypeName),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, showPlayerComm),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, new BetListMetaModel()),
                    Platform = PlatformType.GetPlatformName(ticket.BetFrom),
                    Live = _betlistHtmlBuilder.BuildLiveStatus(ticket)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
    }
}