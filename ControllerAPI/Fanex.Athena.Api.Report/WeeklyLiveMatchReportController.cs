// --------------------------------------------------------------------------
// <copyright file="ApiWeeklyLiveMatchReportController.cs" company="">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class WeeklyLiveMatchReportController : ApiReportBaseController
    {
        private readonly IWeeklyLiveMatchService _weeklyLiveMatchService;
        
        public WeeklyLiveMatchReportController(
            IWeeklyLiveMatchService service,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService)
            : base(sessionService, productService, langService)
        {
            _weeklyLiveMatchService = service;
        }

        [HttpPost]
        public async Task<dynamic> Index(WeeklyLiveMatchMetaModel metaModel)
        {
            if (IsValidRequest(metaModel.CurrencyId))
            {
                var result = await _weeklyLiveMatchService.GetData(metaModel);

                var weekHeaders = _weeklyLiveMatchService.GetWeekHeaders(metaModel.FromDate, metaModel.ToDate, result.NumberOfWeeks);
                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result, weekHeaders);
                }

                if (result.DataList.Any())
                {
                    return new
                    {
                        WeekHeaders = weekHeaders,
                        NumberOfWeeks = result.NumberOfWeeks,
                        Data = result.DataList.Select(MapToGridView).ToArray(),
                        TotalData = result.TotalData
                    };
                }
            }

            return new
            {
                WeekHeaders = new List<string>(),
                NumberOfWeeks = 0,
                Data = new object[0],
                TotalData = new int[0]
            };
        }

        private static object MapToGridView(WeeklyLiveMatch item)
        {
            var data = new List<object> { item.Index, item.SportName };
            data.AddRange(item.NumbersOfMatches.Cast<object>());
            return new
            {
                Cell = data
            };
        }

        protected internal virtual WeeklyLiveMatchExcel CreateExcel(
            string templatePath,
            WeeklyLiveMatchMetaModel metaModel,
            WeeklyLiveMatchResultModel resultModel,
            IEnumerable<string> weekHeaders)
        {
            return new WeeklyLiveMatchExcel(templatePath, metaModel, resultModel, weekHeaders);
        }

        private HttpResponseMessage ExportExcel(
            WeeklyLiveMatchMetaModel metaModel,
            WeeklyLiveMatchResultModel resultModel,
            IEnumerable<string> weekHeaders)
        {
            const string FileName = "WeeklyLiveMatches.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/WeeklyLiveMatch.xls");

            var excel = CreateExcel(templatePath, metaModel, resultModel, weekHeaders);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }
    }
}