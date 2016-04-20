// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    public class LicenseeWinLossReportController : ReportBaseController
    {
        private const string LicenseeWinLossName = "LicenseeWinLoss";

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            AddNotificationMessageToViewBag();

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, LicenseeWinLossName);
            var productList = productAccess.ProductList;
            var metaModel = new LicenseeWinLossMetaModel
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                DrawFilter = DrawFilterType.Normal,
                ReportBy = ReportByType.Daily,
                ProductOptions = productList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = i.ProductCode == Product.SB
                }).ToList(),
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new LicenseeWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = GetUserCurrencies(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
            };

            return View("~/Views/Report/LicenseeWinLoss/Main.cshtml", model);
        }
    }
}