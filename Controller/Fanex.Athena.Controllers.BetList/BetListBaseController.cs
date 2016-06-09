// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.BetList;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Resources;

    #endregion Using

    public class BetListBaseController : ReadonlyBaseController
    {
        protected const int MicroGamingRngTransTypeYesterday = 0;

        protected const int MicroGamingRngTransTypeTodayRunning = 3;

        protected const int PageSizeExcel = 63000;
        protected const int MaxPageSizeDailyBetlist = int.MaxValue - 1000;

        protected const int LicenseeId12Bet = 1;

        protected const int ExternalIdColumn = 1;
        protected const int SystemIdColumn = 2;
        protected const int WinLostColumn = 7;
        protected const int BetEffectiveColumn = 8;
        protected const int LicenseeWLColumn = 9;

        #region Fields

        /// <summary>
        /// Hidden column indexes. WinLoss: 7, LicenseeWL: 8.
        /// </summary>
        protected static readonly ReadOnlyCollection<int> HiddenColumnIndexes =
            new ReadOnlyCollection<int>(new List<int>() { WinLostColumn, BetEffectiveColumn, LicenseeWLColumn });

        /// <summary>
        /// The list contains all bet type of racing.
        /// </summary>
        protected static readonly int[] RacingBetType = { 31, 32, 33, 41, 42, 43 };

        private readonly IBalanceUpService _balanceUpService;

        private readonly IProductService _productService;

        private readonly IBetListService _betListService;

        private readonly ITicketHelper _ticketHelper;

        #endregion Fields

        public BetListBaseController()
        {
            this._productService = new ProductService();
            this._betListService = new BetListService();
            this._ticketHelper = new TicketHelper();
            this._balanceUpService = new BalanceUpService();
        }

        protected IBalanceUpService BalanceUpService
        {
            get { return _balanceUpService; }
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected IBetListService BetListService
        {
            get { return _betListService; }
        }

        protected ITicketHelper TicketHelper
        {
            get { return _ticketHelper; }
        }

        /// <summary>
        /// Set the Licensee Id, Balance Up Date.
        /// Remove the UTC part of DateTime value.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The bet list metal model.</returns>
        public BetListMetaModel CreateDefaultMetaModel(BetListMetaModel metaModel = null)
        {
            metaModel = metaModel ?? new BetListMetaModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var balanceUpInfo = this.BalanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.FromDate = DateTime.SpecifyKind(metaModel.FromDate.Date, DateTimeKind.Utc);
            metaModel.ToDate = DateTime.SpecifyKind(metaModel.ToDate.Date, DateTimeKind.Utc);
            metaModel.Date = DateTime.SpecifyKind(metaModel.Date.Date, DateTimeKind.Utc);

            return metaModel;
        }

        protected static bool IsRacing(int betType)
        {
            return RacingBetType.Contains(betType);
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
                SessionService.UserSession.CurrencyList.Values.Contains(currencyId) ||
                (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean && currencyId == CurrencyType.Cny);
        }
    }
}