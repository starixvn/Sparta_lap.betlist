// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public partial class CasinoProgressiveWinsReportController : ReportController
    {
        private readonly ICasinoProgressiveWinsService _casinoProgressiveWinsService;

        public CasinoProgressiveWinsReportController()
        {
            _casinoProgressiveWinsService = new CasinoProgressiveWinsService();
        }

        #region Action

        [HttpPost]
        public dynamic Index(CasinoProgressiveWinsMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        #endregion Action

        #region Private Utility

        private static CasinoProgressiveWinsGridViewModel MapToGridView(CasinoProgressiveWins item)
        {
            return new CasinoProgressiveWinsGridViewModel
            {
                Index = item.Index,
                SystemId = item.SystemId,
                ExternalId = item.ExternalId,
                RefNo = item.RefNo,
                WinlostDate = item.WinlostDate,
                Choice = "Test",
                WinAmount = item.WinAmount
            };
        }

        private HttpResponseMessage ExportExcel(CasinoProgressiveWinsMetaModel metaModel, IEnumerable<CasinoProgressiveWins> dataList)
        {
            const string FileName = "CasinoProgressiveWins.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/CasinoProgressiveWins.xls");

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var excelModel = new CasinoProgressiveWinsViewModel
            {
                DataList = dataList
            };

            var excel = new CasinoProgressiveWinsExcel(templatePath, FileName, convertingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(CasinoProgressiveWinsMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;

                var dataList = _casinoProgressiveWinsService.GetDataList(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList);
                }

                if (dataList.Count() > 0)
                {
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList()
                    };
                }
            }

            return new
            {
                Data = new List<CasinoProgressiveWinsGridViewModel>()
            };
        }

        #endregion Private Utility
    }
}