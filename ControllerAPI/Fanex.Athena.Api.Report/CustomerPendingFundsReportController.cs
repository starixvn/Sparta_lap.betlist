namespace Fanex.Athena.Api.Report
{
    using System;
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

    [ApiFunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class CustomerPendingFundsReportController : ApiReportBaseController
    {
        private const int PageSizeExcel = int.MaxValue - 1000;
        private readonly ICustPendingFundsService _custPendingFundsService;

        public CustomerPendingFundsReportController()
        {
            _custPendingFundsService = new CustomerPendingFundsService();
        }

        [HttpPost]
        public dynamic Index(CustomerPendingFundsMetaModel metaModel)
        {
            return metaModel.IsExportExcel ? ExportExcel(metaModel) : ProcessRequest(metaModel);
        }

        private dynamic ProcessRequest(CustomerPendingFundsMetaModel metaModel)
        {
            var totalData = new CustPendingFunds();
            var totalRows = 0;
            var dataList = GetData(metaModel, ref totalData, ref totalRows);

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

            var templatePath = HttpContext.Current.Server.MapPath(TemplatePath);

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var totalData = new CustPendingFunds();
            var totalRows = 0;
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var dataList = GetData(metaModel, ref totalData, ref totalRows);

            var custPendingFundsExcel = new CustomerPendingFundsExcel(templatePath, convertingOption, dataList, totalData);
            custPendingFundsExcel.Export();

            return ExcelStream(custPendingFundsExcel.Workbook, FileName);
        }

        private IEnumerable<CustPendingFunds> GetData(CustomerPendingFundsMetaModel metaModel, ref CustPendingFunds totalData, ref int totalRow)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var dataList = _custPendingFundsService.GetDataList(metaModel, ref totalData, ref totalRow);
            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase));

            return dataList;
        }
    }
}