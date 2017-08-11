namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustomerWinlossService _customerWinLossService;

        public CustomerWinLossReportController()
        {
            _customerWinLossService = new CustomerWinlossService();
        }

        [HttpPost]
        public dynamic Index(CustomerWinLossMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(CustomerWinLossMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);
            CustomerWinLoss totalData;
            int totalRows;
            var custWinlossService = new CustomerWinlossService();
            var dataList = custWinlossService.GetDataList(metaModel, out totalData, out totalRows);

            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            var index = 1;
            foreach (var item in dataList)
            {
                item.Index = index++;
            }

            return new
            {
                Data = dataList,
                Total = totalData,
                TotalRows = totalRows
            };
        }

        private HttpResponseMessage ExportExcel(CustomerWinLossMetaModel metaModel)
        {
            const string FileName = "CustomerWinLoss.xls";
            const string TemplatePath = @"~/App_Data/Template/CustomerReports/CustomerWinLoss.xls";

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
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var totalData = new CustomerWinLoss();
            var totalRows = 0;
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            var excel = new CustomerWinLossExcel(templatePath, showingOption, convertingOption, metaModel, dataList, totalData);
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private SortableList<CustomerWinLoss> GetData(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRow)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);
            return _customerWinLossService.GetDataList(metaModel, out totalData, out totalRow);
        }
    }
}