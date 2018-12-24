namespace Fanex.Athena.Api.BetList
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterController : BaseApiController
    {
        private readonly IBettingTicketCenterService _service;

        private readonly IBettingTicketCenterExcel _bettingTicketCenterExcel;

        public BettingTicketCenterController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBettingTicketCenterService service,
            IBettingTicketCenterExcel bettingTicketCenterExcel)
            : base(sessionService, productService, langService)
        {
            _service = service;
            _bettingTicketCenterExcel = bettingTicketCenterExcel;
        }

        [HttpPost]
        public dynamic GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            TicketResult ticketResult;
            if (!metaModel.IsValid || !_service.IsValidCurrencyId(metaModel.CurrencyId))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                ticketResult = _service.GetBetListForSingleCustomer(metaModel);
            }

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = true,
                    ShowSumStake = true,
                    ShowLicenseeWl = true,
                    ShowWinLoss = true,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = _service.BuildSumWinLoss(ticketResult),
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
                ticketResult = _service.GetBetListByRefno(metaModel);
            }

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = false,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel)
            };
        }

        [HttpPost]
        public dynamic GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            TicketResult ticketResult;
            if (!metaModel.IsValid || !_service.IsValidCurrencyId(metaModel.CurrencyId))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                ticketResult = _service.GetRunningBetList(metaModel);
            }

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    ShowTotal = true,
                    ShowSumStake = true,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel),
                Total = new
                {
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
        }

        private HttpResponseMessage Export(BettingTicketCenterExcelModel excelModel)
        {
            if (excelModel.Data.MainTicketList.Count == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            var workbook = _bettingTicketCenterExcel.Export(excelModel);
            return ExcelStream(workbook, string.Empty);
        }
    }
}