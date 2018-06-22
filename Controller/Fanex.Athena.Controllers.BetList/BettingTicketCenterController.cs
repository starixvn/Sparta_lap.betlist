namespace Fanex.Athena.Controllers.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    public class BettingTicketCenterController : BaseController
    {
        private readonly IProductService _productService;

        public BettingTicketCenterController()
        {
            _productService = new ProductService();
        }

        public ActionResult Index()
        {
            return MultipleCustomers();
        }

        public ActionResult MultipleCustomers()
        {
            return View("~/Views/BetList/BettingTicketCenter/Main.cshtml");
        }

        public ActionResult SingleCustomer()
        {
            return View("~/Views/BetList/BettingTicketCenter/Main.cshtml");
        }

        public ActionResult RefNo()
        {
            return View("~/Views/BetList/BettingTicketCenter/RefNo.cshtml");
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

            return View("~/Views/BetList/BettingTicketCenter/Running.cshtml", viewModel);
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
                FunctionNames.BettingTicketCenter);

            return productAccess.ProductList.Select(i => new ProductOption
            {
                Code = i.ProductCode,
                Name = i.ProductName,
                Status = true
            }).ToList();
        }
    }
}