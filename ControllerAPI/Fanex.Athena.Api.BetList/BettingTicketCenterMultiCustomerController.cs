namespace Fanex.Athena.Api.BetList
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
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
    public class BettingTicketCenterMultiCustomerController : BaseApiController
    {
        private const int MaxRowSupportExportExcel = 30;

        private readonly IBettingTicketCenterService _service;

        private readonly IBettingTicketCenterExcel _bettingTicketCenterExcel;

        public BettingTicketCenterMultiCustomerController(
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
        public dynamic GetByMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            TicketResult ticketResult;
            if (!metaModel.IsValid || !_service.IsValidCurrencyId(metaModel.CurrencyId))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                if (metaModel.IsExport && _service.GetTotalRowForMultipleCustomers(metaModel) > MaxRowSupportExportExcel)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }

                ticketResult = _service.GetBetListForMultipleCustomers(metaModel);
            }

            var showPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;

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

        private HttpResponseMessage Export(BettingTicketCenterExcelModel excelModel)
        {
            var filePath = HttpContext.Current.Server.MapPath("~/tmp/abc");
            Directory.CreateDirectory(filePath);
            filePath += "/excel.xls";
            var workbook = _bettingTicketCenterExcel.Export(excelModel);
            FileStream xfile = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write);
            workbook.Write(xfile);
            xfile.Close();
            return ExcelStreamFromFile(filePath, string.Empty);
        }

        private HttpResponseMessage ExcelStreamFromFile(string filePath, string exportFileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = exportFileName
            };
            return result;
        }
    }
}