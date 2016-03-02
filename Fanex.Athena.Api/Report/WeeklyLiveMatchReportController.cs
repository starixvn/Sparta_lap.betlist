// --------------------------------------------------------------------------
// <copyright file="ApiWeeklyLiveMatchReportController.cs" company="Nexcel Solutions Vietnam">
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
    using Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Report;
    using Models.ViewModel.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public class WeeklyLiveMatchReportController : ApiReportBaseController
    {
        private readonly IWeeklyLiveMatchService _weeklyLiveMatchService;

        public WeeklyLiveMatchReportController()
        {
            _weeklyLiveMatchService = new WeeklyLiveMatchService();
        }

        [HttpPost]
        public dynamic Index(WeeklyLiveMatchMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;
                WeeklyLiveMatch total;
                var dataList = _weeklyLiveMatchService.GetData(metaModel, out total);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, total);
                }

                if (dataList.Any())
                {
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToArray(),
                        TotalData = new object[]
                        {
                            13333, 2444, 3, 4555, 5, 6444
                        }
                    };
                }
            }

            return new
            {
                Data = new object[0]
            };
        }

        private static object MapToGridView(WeeklyLiveMatch item)
        {
            var data = new List<object> { item.Index, item.SportName };
            data.AddRange(item.NumOfMatches.Cast<object>());
            return new
            {
                Cell = data
            };
        }

        private HttpResponseMessage ExportExcel(WeeklyLiveMatchMetaModel metaModel, IEnumerable<WeeklyLiveMatch> dataList, WeeklyLiveMatch totalData)
        {
            const string FileName = "WeeklyLiveMatch.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/WeeklyLiveMatch.xls");
            
            var excelModel = new WeeklyLiveMatchViewModel
            {
                Data = dataList,
                TotalData = totalData,
            };

            var excel = new WeeklyLiveMatchExcel(templatePath, FileName, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }
    }
}