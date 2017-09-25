namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Configuration;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossReportController : ApiReportBaseController
    {
        private readonly ICustWinlossDataAccess _custWinlossDataAccess;

        public CustomerWinLossReportController()
        {
            var balanceUpService = new BalanceUpService();
            _custWinlossDataAccess = new CustomerWinlossDataAccess(balanceUpService.BalanceUpInfo);
        }

        [HttpPost]
        public dynamic Index(CustomerWinLossMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(CustomerWinLossMetaModel metaModel)
        {
            CustomerWinLoss totalData;
            int totalRows;
            var dataList = this.GetData(metaModel, out totalData, out totalRows);

            var index = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
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
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                CSVisible = productAccess.IsAllowCS,
                BGVisible = productAccess.IsAllowBG,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            CustomerWinLoss totalData;
            var totalRows = 0;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            var excel = new CustomerWinLossExcel(templatePath, showingOption, convertingOption, metaModel, dataList, totalData);
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private SortableList<CustomerWinLoss> GetData(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRow)
        {
            totalRow = 0;
            totalData = new CustomerWinLoss();

            if (!metaModel.IsValid)
            {
                return new SortableList<CustomerWinLoss>();
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);
            var dataList = _custWinlossDataAccess.GetDataList(metaModel, out totalData, out totalRow);
            if (!string.IsNullOrEmpty(metaModel.SortColumn))
            {
                dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            }

            return dataList;
        }
    }
}