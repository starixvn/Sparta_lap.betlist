namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.ObjectModel;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Helpers;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.BetList;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DTO.Configuration;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class BetListBaseController : BaseController
    {
        protected const int PageSizeExcel = 63000;

        protected const int ExternalIdColumn = 1;
        protected const int SystemIdColumn = 2;
        protected const int WinLostColumn = 7;
        protected const int BetEffectiveColumn = 8;
        protected const int LicenseeWLColumn = 9;

        protected static readonly ReadOnlyCollection<int> HiddenColumnIndexes =
            new ReadOnlyCollection<int>(new[] { WinLostColumn, BetEffectiveColumn, LicenseeWLColumn });

        protected static readonly ReadOnlyCollection<int> RacingBetType =
            new ReadOnlyCollection<int>(new[] { 31, 32, 33, 41, 42, 43 });

        private readonly IAuthorizationService _authorizationService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IProductService _productService;

        private readonly IBetListService _betListService;

        public BetListBaseController()
        {
            _authorizationService = new AuthorizationService();
            this._productService = new ProductService();
            this._betListService = new BetListService();
            this._balanceUpDataAccess = new BalanceUpDataAccess();
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected IBetListService BetListService
        {
            get { return _betListService; }
        }

        protected static bool IsRacing(int betType)
        {
            return RacingBetType.Contains(betType);
        }

        protected static string GetTitleByTransType(string productTitle, long transType)
        {
            const int TransTypeYesterday = 0;
            const int TransTypeTodayRunning = 3;

            var title = productTitle + ViewResources.BetList + " - " + ViewResources.DetailTickets + " - ";
            switch (transType)
            {
                case TransTypeYesterday:
                    title += ViewResources.Yesterday;
                    break;

                case TransTypeTodayRunning:
                    title += ViewResources.Today + " - " + ViewResources.Running;
                    break;

                default:
                    title += ViewResources.Today + " - " + ViewResources.Settled;
                    break;
            }

            return title;
        }

        protected static string GetViewInCurrency(int curencyId, string defaultCurrencyName)
        {
            string viewinCurrency;
            switch (curencyId)
            {
                case BaseCurrencyType.All:
                    viewinCurrency = ViewResources.AllCurrencies;
                    break;

                case BaseCurrencyType.Eur:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Eur);
                    break;

                case BaseCurrencyType.Usd:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Usd);
                    break;

                case BaseCurrencyType.Gbp:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Gbp);
                    break;

                default:
                    viewinCurrency = string.Format(ViewResources.Viewin, defaultCurrencyName);
                    break;
            }

            return viewinCurrency;
        }

        protected bool HasPermissionOnData(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId);
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }

        /// <summary>
        /// Set the Licensee Id, Balance Up Date.
        /// Remove the UTC part of DateTime value.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The bet list metal model.</returns>
        protected BetListMetaModel CreateDefaultMetaModel(BetListMetaModel metaModel = null)
        {
            var defaultMetaModel = metaModel ?? new BetListMetaModel();
            defaultMetaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            defaultMetaModel.UserId = SessionService.UserSession.UserId;

            var balanceUp = GetBalanceUp();
            defaultMetaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            defaultMetaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            defaultMetaModel.FromDate = DateTime.SpecifyKind(defaultMetaModel.FromDate.Date, DateTimeKind.Utc);
            defaultMetaModel.ToDate = DateTime.SpecifyKind(defaultMetaModel.ToDate.Date, DateTimeKind.Utc);
            defaultMetaModel.Date = DateTime.SpecifyKind(defaultMetaModel.Date.Date, DateTimeKind.Utc);

            defaultMetaModel.IsFakeLicensee = IsFakeLicensee();
            return defaultMetaModel;
        }

        /// <summary>
        /// Override the max JSON Length.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentEncoding">The encoding.</param>
        /// <param name="behavior">The JSON request behavior.</param>
        /// <returns>The JSON result.</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected bool IsValidCurrency(int currencyId)
        {
            return currencyId == BaseCurrencyType.All ||
                KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(
                    SessionService.UserSession.CurrencyList, SessionService.LicenseeSession.LicenseeId, currencyId);
        }

        protected FileContentResult EmptyExcel()
        {
            return this.File(ExcelHelper.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        protected virtual ActionResult ExportExcel(BetListMetaModel metaModel)
        {
            return null;
        }

        protected ActionResult ProcessDefaultRequest(BetListMetaModel metaModel, string viewPath)
        {
            var model = CreateDefaultMetaModel(metaModel);

            if (!model.IsExportExcel)
            {
                return View(viewPath, model);
            }

            if (IsValidCurrency(model.CurrencyId))
            {
                model.AgentListXml = SessionService.GenerateAgentListXml(model.CurrencyId);
                return ExportExcel(model);
            }

            return EmptyExcel();
        }
    }
}