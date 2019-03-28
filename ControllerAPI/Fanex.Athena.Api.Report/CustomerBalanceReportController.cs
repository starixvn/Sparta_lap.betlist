namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerBalance)]
    public class CustomerBalanceReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustBalanceDataAccess _custBalanceDataAccess;

        public CustomerBalanceReportController(
            ICustBalanceDataAccess custBalanceDataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _custBalanceDataAccess = custBalanceDataAccess;
        }

        [HttpPost]
        public dynamic Index(CustomerBalanceMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        protected internal virtual CustomerBalanceExcel CreateExcel(
            string templatePath,
            CustomerBalanceMetaModel metaModel,
            ReportResultModel<CustBalance> result,
            ShowingOption showingOption,
            ConvertingOption convertingOption)
        {
            return new CustomerBalanceExcel(templatePath, metaModel, result.DataList, result.TotalData, showingOption, convertingOption);
        }

        private dynamic ProcessRequest(CustomerBalanceMetaModel metaModel)
        {
            var result = GetData(metaModel);

            return new
            {
                Data = result.DataList,
                Total = result.TotalData,
                TotalRows = result.TotalRows
            };
        }

        private async Task<HttpResponseMessage> ExportExcel(CustomerBalanceMetaModel metaModel)
        {
            const string FileName = "CustomerBalance.xls";
            const string TemplatePath = @"~/App_Data/Template/CustomerReports/CustomerBalance.xls";

            var templatePath = HostingEnvironment.MapPath(TemplatePath);

            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = await ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId, 
                SessionService.UserSession.Language, 
                FunctionNames.CustomerBalance)
                .ConfigureAwait(false);

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var result = GetData(metaModel);

            var custBalanceExcel = CreateExcel(templatePath, metaModel, result, showingOption, convertingOption);
            custBalanceExcel.Export();

            return ExcelStream(custBalanceExcel.Workbook, FileName);
        }

        private ReportResultModel<CustBalance> GetData(CustomerBalanceMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<CustBalance>
                {
                    TotalData = new CustBalance(),
                    TotalRows = 0,
                    DataList = new List<CustBalance>()
                };
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);
            return _custBalanceDataAccess.GetDataList(metaModel);
        }
    }
}