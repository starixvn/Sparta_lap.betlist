namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerBalance)]
    public class CustomerBalanceReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustomerBalanceService _custBalanceService;

        public CustomerBalanceReportController()
        {
            _custBalanceService = new CustomerBalanceService();
        }

        [HttpPost]
        public dynamic Index(CustomerBalanceMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(CustomerBalanceMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, FunctionNames.CustomerBalance);

            var totalData = new CustBalance();
            var totalRows = 0;
            var dataList = GetData(metaModel, ref totalData, ref totalRows);

            return new
            {
                Data = dataList,
                Total = totalData,
                HasCustomerComm = licenseeSession.IsAllowedCustomerComm,
                IsAllowBA = productAccess.IsAllowBA,
                TotalRows = totalRows
            };
        }

        private HttpResponseMessage ExportExcel(CustomerBalanceMetaModel metaModel)
        {
            const string FileName = "CustomerBalance.xls";
            const string TemplatePath = @"~/App_Data/Template/CustomerReports/CustomerBalance.xls";

            var templatePath = HttpContext.Current.Server.MapPath(TemplatePath);

            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, FunctionNames.CustomerBalance);

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var totalData = new CustBalance();
            var totalRows = 0;
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var dataList = GetData(metaModel, ref totalData, ref totalRows);

            var custBalanceExcel = new CustomerBalanceExcel(templatePath, metaModel, dataList, totalData, showingOption, convertingOption);
            custBalanceExcel.Export();

            return ExcelStream(custBalanceExcel.Workbook, FileName);
        }

        private IEnumerable<CustBalance> GetData(CustomerBalanceMetaModel metaModel, ref CustBalance totalData, ref int totalRow)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);
            return _custBalanceService.GetDataList(metaModel, ref totalData, ref totalRow);
        }
    }
}