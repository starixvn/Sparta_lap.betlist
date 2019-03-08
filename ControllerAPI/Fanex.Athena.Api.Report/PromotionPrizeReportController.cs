namespace Fanex.Athena.Api.Report
{
    using System.Net.Http;
    using System.Web.Hosting;
    using Base.Attributes;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Excel.Report;
    using Excel.Report.Helpers;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report;
    using Model.Report.MetaModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss, PermissionKeys.ReportCustomerWinLoss)]
    public class PromotionPrizeReportController : ApiReportBaseController
    {
        private readonly IPromotionPrizeDataAccess _dataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public PromotionPrizeReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IPromotionPrizeDataAccess dataAccess,
            ICacheRefAdapter cacheRefAdapter)
            : base(sessionService, productService, langService)
        {
            _dataAccess = dataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        protected internal virtual HttpResponseMessage CreareExcel(string template, string title, string fileName, ReportResultModel<PromotionPrize> data)
        {
            var excel = new PromotionPrizeExcel(template, title, data);
            excel.Export();

            return ExcelStream(excel.Workbook, fileName);
        }

        public HttpResponseMessage ExportExcel(PromotionPrizeMetaModel metaModel)
        {
            const string FileName = "PP_PromotionPrizeWin.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/PP_PromotionPrizeWin.xls");

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);

            var data = ValidateRequest(metaModel)
                ? _dataAccess.GetData(metaModel, SessionService.UserSession.CurrencyList)
                : new ReportResultModel<PromotionPrize>();

            var title = ReportExcelHelper.GenerateReportTitle(
                ViewResources.ProductPP + " - " + _cacheRefAdapter.GetBettype(BetType.PromotionPrize),
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.CurrencyId <= 0 ? ViewResources.AllCurrencies : metaModel.CurrencyName);

            return CreareExcel(templatePath, title, FileName, data);
        }

        private bool ValidateRequest(PromotionPrizeMetaModel metaModel)
        {
            return metaModel.FromDate <= metaModel.ToDate && IsValidRequest(metaModel.CurrencyId);
        }
    }
}