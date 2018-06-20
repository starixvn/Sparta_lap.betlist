namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Service.BetList;
    using Service.BetList.Interfaces;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    public class BettingTicketsCenterController : BaseController
    {
        private readonly IProductService _productService;

        private readonly IBettingTicketsCenterService _service;

        public BettingTicketsCenterController()
        {
            _productService = new ProductService();
            _service = new BettingTicketsCenterService();
        }

        public ActionResult MultipleCustomers()
        {
            return View("~/Views/BetList/BettingTicketsCenter/Main.cshtml");
        }

        public ActionResult SingleCustomer()
        {
            return View("~/Views/BetList/BettingTicketsCenter/Main.cshtml");
        }

        public ActionResult RefNo()
        {
            return View("~/Views/BetList/BettingTicketsCenter/RefNo.cshtml");
        }

        public ActionResult Running()
        {
            var viewModel = new RunningBetListViewModel
            {
                CurrencyList = GetCurrencyList(),
                ProductOptionList = GetProductOptionList(),
                MinDate = AppSettings.HistoryDate,
                IsAllowedCustomerComm = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/BettingTicketsCenter/Running.cshtml", viewModel);
        }

        public ActionResult ExportRefNo(RefNoBetListMetaModel metaModel)
        {
            return Export("RefNo");
        }

        private ActionResult Export(string fileName)
        {
            return File(ExcelHelper.ExportEmptyFile(), "application/vnd.ms-excel", $"{fileName}.xls");
        }

        private IDictionary<int, string> GetCurrencyList()
        {
            return SessionService.UserSession.CurrencyList;
        }

        private IEnumerable<ProductOption> GetProductOptionList()
        {
            var productAccess = _productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.BettingTicketsCenter);

            return productAccess.ProductList.Select(i => new ProductOption
            {
                Code = i.ProductCode,
                Name = i.ProductName,
                Status = true
            }).ToList();
        }
    }
}