namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerBalance)]
    public class CustomerBalanceReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustBalanceDataAccess _custBalanceDataAccess;

        public CustomerBalanceReportController()
        {
            _custBalanceDataAccess = new CustomerBalanceDataAccess();
        }

        public CustomerBalanceReportController(
            ICustBalanceDataAccess custBalanceDataAccess,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _custBalanceDataAccess = custBalanceDataAccess;
        }

        [HttpPost]
        public dynamic Index(CustomerBalanceMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        [CLSCompliant(false)]
        protected internal virtual CustomerBalanceExcel CreateExcel(
            string templatePath,
            CustomerBalanceMetaModel metaModel,
            IEnumerable<CustBalance> dataList,
            CustBalance totalData,
            ShowingOption showingOption,
            ConvertingOption convertingOption)
        {
            return new CustomerBalanceExcel(templatePath, metaModel, dataList, totalData, showingOption, convertingOption);
        }

        private dynamic ProcessRequest(CustomerBalanceMetaModel metaModel)
        {
            CustBalance totalData;
            int totalRows;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            return new
            {
                Data = dataList,
                Total = totalData,
                TotalRows = totalRows
            };
        }

        private HttpResponseMessage ExportExcel(CustomerBalanceMetaModel metaModel)
        {
            const string FileName = "CustomerBalance.xls";
            const string TemplatePath = @"~/App_Data/Template/CustomerReports/CustomerBalance.xls";

            var templatePath = HostingEnvironment.MapPath(TemplatePath);

            var licenseeSession = SessionService.LicenseeSession;
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, FunctionNames.CustomerBalance);

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

            CustBalance totalData;
            int totalRows;
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            var custBalanceExcel = CreateExcel(templatePath, metaModel, dataList, totalData, showingOption, convertingOption);
            custBalanceExcel.Export();

            return ExcelStream(custBalanceExcel.Workbook, FileName);
        }

        private IEnumerable<CustBalance> GetData(CustomerBalanceMetaModel metaModel, out CustBalance totalData, out int totalRow)
        {
            if (!metaModel.IsValid)
            {
                totalData = new CustBalance();
                totalRow = 0;
                return new List<CustBalance>();
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);
            return _custBalanceDataAccess.GetDataList(metaModel, out totalData, out totalRow);
        }
    }
}