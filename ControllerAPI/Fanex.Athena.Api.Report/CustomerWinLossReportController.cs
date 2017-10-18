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
    using Model.Core.ResultModel;
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
            var result = GetData(metaModel);

            var index = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            foreach (var item in result.DataList)
            {
                item.Index = index++;
            }

            return new
            {
                Data = result.DataList,
                Total = result.TotalData,
                TotalRows = result.TotalRows
            };
        }

        private HttpResponseMessage ExportExcel(CustomerWinLossMetaModel metaModel)
        {
            System.Threading.Thread.Sleep(4000);
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

            var result = GetData(metaModel);
            var excelModel = new CustomerWinLossExcelModel
            {
                ConvertingOption = convertingOption,
                DataList = result.DataList,
                MetaModel = metaModel,
                TotalData = result.TotalData,
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                SystemIdVisible = !_appSettings.FakeLicenseeList.Contains(licenseeSession.LicenseeId)
            };

            var excel = CreateExcel(templatePath, excelModel);
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private SortableReportResultModel<CustomerWinLoss> GetData(CustomerWinLossMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new SortableReportResultModel<CustomerWinLoss>
                {
                    TotalData = new CustomerWinLoss(),
                    DataList = new SortableList<CustomerWinLoss>()
                };
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludedTestData);
            var result = _custWinlossDataAccess.GetDataList(metaModel);
            if (!string.IsNullOrEmpty(metaModel.SortColumn))
            {
                result.DataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }
    }
}