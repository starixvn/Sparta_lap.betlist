using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportLicenseeWinLossByTimeZone)]
    public class LicenseeWinLossByTimeZoneReportController : ReportBaseController
    {
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language);

            var model = new LicenseeWinLossByTimeZoneViewModel
            {
                CurrencyList = userSession.CurrencyList,
                ProductOptionList = productAccess.ProductList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = true
                }).ToList()
            };

            return ViewForNewUI("~/Views/Report/LicenseeWinLossByTimeZone/Main.cshtml", model, "LicenseeWinLossByTimeZoneReport");
        }
    }
}