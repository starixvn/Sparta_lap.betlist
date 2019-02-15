// --------------------------------------------------------------------------
// <copyright file="ApiBetListBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.BetList.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Utility.Helpers;
    using Common.Utility.Services.Interfaces;
    using CustomActionResult;
    using Excel;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Model.ViewModel;

    public class BetListController : BaseController
    {
        protected const int PageSizeExcel = 63000;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IBetListService _betListService;

        public BetListController(ICacheRefAdapter cacheRefAdapter, IBetListService betListService)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _betListService = betListService;
        }

        [HttpGet]
        public string Index()
        {
            return "Betlist Api default page";
        }

        [HttpPost]
        public JsonResult GetBetList(BetListMetaModel metaModel)
        {
            var ticketResult = _betListService.GetBetListData(metaModel);

            var leagueName = string.Empty;
            var firstOrDefault = ticketResult.MainTicketList.FirstOrDefault();
            if (firstOrDefault != null)
            {
                leagueName = _betListService.GetLeagueName(firstOrDefault.LeagueId);
            }

            var result = new BetListResultViewModel
            {
                Main = _betListService.BuildTicketTextList(ticketResult, metaModel, isShowPlayerComm: metaModel.HasPlayerCom),
                Total = new BetListTotalData
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm,
                    SumBetEffective = ticketResult.SumBetEffective,
                    SumStake = ticketResult.MainTicketList.Sum(x => Math.Round(x.Stake, 2))
                },
                LeagueName = leagueName,
            };
            ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetBetListPaging(BetListMetaModel metaModel)
        {
            metaModel.IsServerPaging = true;

            var ticketResult = _betListService.GetBetListData(metaModel);

            var totalPage = (int)Math.Ceiling((double)ticketResult.TotalRows / metaModel.PageSize);
            var pageIndex = metaModel.PageIndex > totalPage ? totalPage : metaModel.PageIndex;

            var startIndex = ((pageIndex - 1) * metaModel.PageSize) + 1;

            var sumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm;
            var sumWinLossText = NumberHelper.DecFormat(ticketResult.SumWinLoss + ticketResult.SumPlayerComm, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(sumLicenseeWl, 2);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative-number'>{0}</span>", sumLicenseeWlText)
                : sumLicenseeWlText;

            var result = new
            {
                page = pageIndex,
                total = totalPage,
                records = ticketResult.TotalRows,
                rows = _betListService.BuildTicketTextList(ticketResult, metaModel, startIndex, isShowPlayerComm: metaModel.HasPlayerCom),
                userdata = new
                {
                    WinLoss = sumWinLossText,
                    LicenseeWL = sumLicenseeWlText
                }
            };

            ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public dynamic GetDailyBetListData(DailyBetListMetaModel metaModel)
        {
            return GetBetListPaging(metaModel);
        }

        [HttpPost]
        public dynamic GetWinLossByLeagueData(WinLossByLeagueBetListMetaModel metaModel)
        {
            return GetBetList(metaModel);
        }

        [HttpPost]
        public ExcelResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            var ticketResult = _betListService.GetBetListData(metaModel);

            return ProcessExportExcel(metaModel, ticketResult);
        }

        [HttpPost]
        public ExcelResult ExportExcelDailyBetList(DailyBetListMetaModel metaModel)
        {
            var ticketResult = _betListService.GetBetListData(metaModel);

            return ProcessExportExcel(metaModel, ticketResult);
        }

        [HttpPost]
        public ExcelResult ExportExcelWinlossByLeague(WinLossByLeagueBetListMetaModel metaModel)
        {
            var ticketResult = _betListService.GetBetListData(metaModel);

            return ProcessExportExcel(metaModel, ticketResult);
        }

        protected internal virtual ExcelResult ProcessExportExcel(
            BetListExcelMetaModel metaModel,
            TicketResult ticketResult)
        {
            const string LeagueNamePlaceHolder = "(LEAGUENAME)";
            if (metaModel.Title.Contains(LeagueNamePlaceHolder))
            {
                var firstTicket = ticketResult.MainTicketList.FirstOrDefault();
                var leagueName = firstTicket != null ? _cacheRefAdapter.GetLeagueName(firstTicket.LeagueId) : string.Empty;
                metaModel.Title = metaModel.Title.Replace(LeagueNamePlaceHolder, leagueName);
            }

            var excel = new BetListExcel(metaModel.ShowingOption, metaModel.LicenseeId, metaModel.HiddenColumns, metaModel.IsShowSumStake);

            var workbook = excel.Export(ticketResult, metaModel.Title, metaModel.StartIndex);

            return new ExcelResult(workbook);
        }
    }
}