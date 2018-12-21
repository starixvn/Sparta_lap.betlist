namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Mvc;
    using Base;
    using Base.Attributes;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Ionic.Zip;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterMultiCustomerController : BaseApiController
    {
        private const int MaxRowSupportExportExcel = 300000;

        private const int MaxRowPerFile = 65000;

        private readonly IBettingTicketCenterService _service;

        public BettingTicketCenterMultiCustomerController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBettingTicketCenterService service)
            : base(sessionService, productService, langService)
        {
            _service = service;
        }

        [System.Web.Http.HttpPost]
        public dynamic GetData(MultipleCustomersBetListMetaModel metaModel)
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

            if (metaModel.IsExport)
            {
                return Export(ticketResult, _service.GetTitle(metaModel));
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

        private HttpResponseMessage Export(TicketResult ticketResult, string title)
        {
            return ticketResult.MainTicketList.Count > MaxRowPerFile ?
                ExportZip(ticketResult, title) :
                ExportXls(ticketResult, title);
        }

        private HttpResponseMessage ExportXls(TicketResult ticketResult, string title)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                Title = title
            };

            var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
            var workbook = excel.Export(excelModel);
            return ExcelStream(workbook, string.Empty);
        }

        private HttpResponseMessage ExportZip(TicketResult ticketResult, string title)
        {
            var dataList = ticketResult.MainTicketList.ToList();
            ticketResult.MainTicketList.Clear();
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                Title = title
            };

            var path = HttpContext.Current.Server.MapPath("~/tmp/BTC/" + Guid.NewGuid());

            try
            {    
                Directory.CreateDirectory(path);

                var numOfFile = (int)Math.Ceiling((double)dataList.Count / MaxRowPerFile);
                for (int i = 0; i < numOfFile; i++)
                {
                    var startIndex = i * MaxRowPerFile;

                    ticketResult.MainTicketList.Clear();
                    ticketResult.MainTicketList.AddRange(dataList.Skip(startIndex).Take(MaxRowPerFile));
                    var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();

                    var filePath = path + $"/sheet{i}.xls";
                    var workbook = excel.Export(excelModel);
                    using (var stream = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        workbook.Write(stream);
                        stream.Close();
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < numOfFile; i++)
                    {
                        var filePath = path + $"/sheet{i}.xls";
                        zip.AddEntry(Path.GetFileName(filePath), new FileStream(filePath, FileMode.Open, FileAccess.Read));
                    }

                    return ZipContentResult(zip, path);
                }
            }
            catch (Exception)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                }

                throw;
            }
        }

        protected HttpResponseMessage ZipContentResult(ZipFile zipFile, string path)
        {
            var pushStreamContent = new PushStreamContent((stream, content, context) =>
            {
                zipFile.Save(stream);
                stream.Close(); // After save we close the stream to signal that we are done writing.
                Directory.Delete(path, true);
            }, "application/zip");

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = pushStreamContent };
        }
    }
}