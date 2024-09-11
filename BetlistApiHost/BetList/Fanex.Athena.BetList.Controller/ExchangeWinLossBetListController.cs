using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;

namespace Fanex.Athena.BetList.Controller
{
    public class ExchangeWinLossBetListController : BaseController
    {
        private readonly IExchangeBetListService _service;
        private readonly IExchangeExcel _exchangeExcel;

        public ExchangeWinLossBetListController(IExchangeBetListService service, IExchangeExcel exchangeExcel)
        {
            _service = service;
            _exchangeExcel = exchangeExcel;
        }

        [HttpPost]
        public async Task<JsonResult> GetData(ExchangeBetListMetaModel metaModel)
        {
            return Json(await _service.GetBetListDataAsync(metaModel).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<JsonResult> Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            return Json(await _service.GetTicketDetailListAsync(metaModel).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<JsonResult> GetRunningTickets(ExchangeRunningTicketMetaModel metaModel)
        {
            var result = await _service.GetRunningTicketsAsync(metaModel);
            return Json(result);
        }

        [HttpPost]
        public async Task<FileContentResult> ExportRunningTickets(ExchangeRunningTicketMetaModel metaModel)
        {
            var data = await _service.GetRunningTicketsAsync(metaModel);
            var template = Server.MapPath("~/App_Data/ExchangeRunningTicket.xls");
            var fileBytes = _exchangeExcel.ExportRunningExcel(template, data, metaModel);

            return File(fileBytes, "application/octet-stream", "ExchangeRunningTicket.xls");
        }
    }
}