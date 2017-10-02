// -----------------------------------------------------------------------
// <copyright file="MatchWinLossBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult MatchBetType(BetListMetaModel metaModel)
        {
            // include LicenseeId, UserId in CreateDefaultMetaModel
            var model  = CreateDefaultMetaModel(metaModel);
            model.IsServerPaging = true;

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                    var uspKey = IsRacing(model.BetType)
                        ? "Lrf_BetList_MatchWL_Racing"
                        : "Lrf_BetList_MatchWL_Bettype";
                    return Excel(model, uspKey);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/MatchWl/MatchBetType.cshtml", model);
        }

        [HttpGet]
        public ActionResult MixParlay(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            model.IsServerPaging = true;

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;
                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                    const string UspKey = "Lrf_BetList_MatchWL_Parlay";
                    model.IsServerPaging = true;
                    return Excel(model, UspKey);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/MatchWl/MixParlay.cshtml", model);
        }

        [HttpGet]
        public ActionResult Outright(BetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            model.IsServerPaging = true;

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                    const string UspKey = "Lrf_BetList_MatchWL_Outright";
                    return Excel(model, UspKey);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/MatchWl/Outright.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = PageSizeExcel;
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                  ? CacheRef.GetTeamName(metaModel.HomeId) + " vs " + CacheRef.GetTeamName(metaModel.AwayId) + " - "
                  : string.Empty;
            var currencyTitle = string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);

            var title = ViewResources.BetList
                + string.Format(
                " - {0}{1} - {2}",
                matchName + (metaModel.BetType > 0 ? CacheRef.GetBettype(metaModel.BetType) + " - " : string.Empty),
                metaModel.FromDate.ToShortDateString(),
                currencyTitle);

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, new List<int> { BetEffectiveColumn });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "MatchWinLossBetList");
        }
    }
}