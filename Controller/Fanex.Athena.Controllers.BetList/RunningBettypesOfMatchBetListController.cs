// -----------------------------------------------------------------------
// <copyright file="RunningBettypesOfMatchBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class RunningBettypesOfMatchBetListController : BetListBaseController
    {
        public RunningBettypesOfMatchBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Index(BetListExcelMetaModel metaModel)
        {
            CreateDefaultMetaModel(metaModel);
            return ProcessDefaultRequest(metaModel, "~/Views/Desktop/BetList/RunningBettypesOfMatchBetList/Main.cshtml", "Lrf_BetList_SelRunningByMultipleBettypeMatch");
        }

        protected override ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            var bettype = metaModel.BetType == 0 && !string.IsNullOrWhiteSpace(metaModel.MultipleBettype)
                ? Converter.ToInt(metaModel.MultipleBettype.Split(',')[0])
                : metaModel.BetType;

            metaModel.Title = $"(LEAGUENAME) - {CacheRefAdapter.GetBettype(bettype)} - {GetViewInCurrency(metaModel)}";
            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            metaModel.HiddenColumns = HiddenColumnIndexes;
            return ProcessExportExcel(metaModel, "RunningBetList");
        }
    }
}