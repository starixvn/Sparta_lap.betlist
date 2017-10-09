namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Excel.Report.Models;
    using Extension.Utility;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossReportController : ApiReportBaseController
    {
        private readonly IAppSettings _appSettings;

        private readonly ICustWinlossDataAccess _custWinlossDataAccess;

        public CustomerWinLossReportController()
        {
            _appSettings = new AppSettings();
            _custWinlossDataAccess = new CustomerWinlossDataAccess();
        }

        public CustomerWinLossReportController(
            IAppSettings appSettings,
            ICustWinlossDataAccess custWinlossDataAccess,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _appSettings = appSettings;
            _custWinlossDataAccess = custWinlossDataAccess;
        }

        [HttpPost]
        public dynamic Index(CustomerWinLossMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        [CLSCompliant(false)]
        protected internal virtual CustomerWinLossExcel CreateExcel(string templatePath, CustomerWinLossExcelModel excelModel)
        {
            return new CustomerWinLossExcel(templatePath, excelModel);
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

            var templatePath = HostingEnvironment.MapPath(TemplatePath);

            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, FunctionNames.CustomerBalance);

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            CustomerWinLoss totalData;
            int totalRows;
            var dataList = GetData(metaModel, out totalData, out totalRows);
            var excelModel = new CustomerWinLossExcelModel
            {
                ConvertingOption = convertingOption,
                DataList = dataList,
                MetaModel = metaModel,
                TotalData = totalData,
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                SystemIdVisible = !_appSettings.FakeLicenseeList.Contains(licenseeSession.LicenseeId)
            };

            var excel = CreateExcel(templatePath, excelModel);
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private SortableList<CustomerWinLoss> GetData(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRow)
        {
            if (!metaModel.IsValid)
            {
                totalRow = 0;
                totalData = new CustomerWinLoss();
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