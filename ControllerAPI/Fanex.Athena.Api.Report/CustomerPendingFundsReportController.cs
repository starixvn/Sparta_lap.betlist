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
    using Extension.Utility;
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class CustomerPendingFundsReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustPendingFundsDataAccess _custPendingFundsDataAccess;

        public CustomerPendingFundsReportController()
        {
            _custPendingFundsDataAccess = new CustomerPendingFundsDataAccess();
        }

        public CustomerPendingFundsReportController(
            ICustPendingFundsDataAccess custPendingFundsDataAccess,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _custPendingFundsDataAccess = custPendingFundsDataAccess;
        }

        [HttpPost]
        public dynamic Index(CustomerPendingFundsMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        [CLSCompliant(false)]
        protected internal virtual CustomerPendingFundsExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            IEnumerable<CustPendingFunds> dataList,
            CustPendingFunds totalData)
        {
            return new CustomerPendingFundsExcel(templatePath, convertingOption, dataList, totalData);
        }

        private dynamic ProcessRequest(CustomerPendingFundsMetaModel metaModel)
        {
            CustPendingFunds totalData;
            int totalRows;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            return new
            {
                Data = dataList,
                Total = totalData,
                TotalRows = totalRows
            };
        }

        private HttpResponseMessage ExportExcel(CustomerPendingFundsMetaModel metaModel)
        {
            const string FileName = "CustomerPendingFunds.xls";
            const string TemplatePath = @"~/App_Data/Template/CustomerReports/CustomerPendingFunds.xls";

            var templatePath = HostingEnvironment.MapPath(TemplatePath);

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            CustPendingFunds totalData;
            int totalRows;
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var dataList = GetData(metaModel, out totalData, out totalRows);

            var custPendingFundsExcel = CreateExcel(templatePath, convertingOption, dataList, totalData);
            custPendingFundsExcel.Export();

            return ExcelStream(custPendingFundsExcel.Workbook, FileName);
        }

        private IEnumerable<CustPendingFunds> GetData(CustomerPendingFundsMetaModel metaModel, out CustPendingFunds totalData, out int totalRows)
        {
            if (!metaModel.IsValid)
            {
                totalData = new CustPendingFunds();
                totalRows = 0;
                return new SortableList<CustPendingFunds>();
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var dataList = _custPendingFundsDataAccess.GetDataList(metaModel, out totalData, out totalRows);
            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase));

            return dataList;
        }
    }
}