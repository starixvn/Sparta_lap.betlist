// -----------------------------------------------------------------------
// <copyright file="BetSettingController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings.RngCasino;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.BetSettings;
    using Fanex.Athena.Models.ViewModel.BetSettings;
    using Fanex.Athena.Service.Implement.Casino;
    using Fanex.Athena.Service.Interface.Casino;
    using Fanex.Athena.Web.ActionFilter;
    using Service.Implement.BetSettings;
    using Service.Interface.BetSettings;

    public class BetSettingController : BetSettingBaseController
    {
        private readonly IBetSettingService _betSettingService;
        private readonly ICasinoSetting _casinoSetting;

        public BetSettingController()
        {
            this._betSettingService = new BetSettingService();
            this._casinoSetting = new CasinoSetting();
        }

        #region Displaying bet settings

        /// <summary>
        /// The home page.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [FunctionAuthorization(ParentUrl = CustomerListUrl)]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = GetViewModels(metaModel);

            if (viewModel == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            switch (metaModel.ProductId)
            {
                case ProductTypes.NumberGame:
                    return View("~/Views/Customer/BetSetting/NumberGame.cshtml", viewModel);

                case ProductTypes.LiveCasino:
                    return View("~/Views/Customer/BetSetting/LiveCasino.cshtml", viewModel);

                case ProductTypes.RngCasino:
                    return View("~/Views/Customer/BetSetting/RngCasino.cshtml", viewModel);

                case ProductTypes.Keno:
                    return View("~/Views/Customer/BetSetting/Keno.cshtml", viewModel);

                case ProductTypes.AgCasino:
                    return View("~/Views/Customer/BetSetting/AgCasino.cshtml", viewModel);

                case ProductTypes.GoldDeluxe:
                    return View("~/Views/Customer/BetSetting/GoldDeluxe.cshtml", viewModel);

                case ProductTypes.Macau:
                    return View("~/Views/Customer/BetSetting/Macau.cshtml", viewModel);

                case ProductTypes.MicroGamingLiveCasino:
                    return View("~/Views/Customer/BetSetting/MicroGamingLiveCasino.cshtml", viewModel);

                case ProductTypes.MicrogamingRNGCasino:
                    return View("~/Views/Customer/BetSetting/MicroGamingRNGCasino.cshtml", viewModel);

                case ProductTypes.BetTrade:
                    return View("~/Views/Customer/BetSetting/BetTrade.cshtml", viewModel);

                default: // Sportsbook, Racing, Virtual Sports
                    return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
            }
        }

        #endregion Displaying bet settings

        #region Common

        private static bool? IsLiveCasinoEnabled(CustomerInfo customerInfo)
        {
            bool? resultValue = false;
            if (customerInfo.DisableLiveCasino == false && customerInfo.SyncLiveCasino)
            {
                resultValue = true;
            }
            else if (customerInfo.SyncLiveCasino == false)
            {
                resultValue = null;
            }

            return resultValue;
        }

        private static bool? IsRngCasinoEnabled(CustomerInfo customerInfo)
        {
            bool? resultValue = false;
            if (customerInfo.DisableCasino == false && customerInfo.SyncCasino)
            {
                resultValue = true;
            }
            else if (customerInfo.SyncCasino == false)
            {
                resultValue = null;
            }

            return resultValue;
        }

        private BetSettingViewModel GetViewModels(BetSettingMetaModel metaModel)
        {
            var customerInfo = CustomerService.GetCustomerInfo(metaModel.CustId);
            var viewModel = GetViewModel(metaModel.ProductId, metaModel.CustId, customerInfo);
            if (viewModel == null)
            {
                return null;
            }

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            AssignRequiredProperties(metaModel);

            metaModel.IsAllowSportsBook = ProductAccess.IsAllowSB;
            metaModel.IsAllowRacing = ProductAccess.IsAllowRB;
            metaModel.IsAllowVirtualSport = ProductAccess.IsAllowVS;
            metaModel.IsAllowNumberGame = ProductAccess.IsAllowNG;
            metaModel.IsAllowRngCasino = ProductAccess.IsAllowCS && customerInfo.SyncCasino;
            metaModel.IsAllowLiveCasino = ProductAccess.IsAllowLC && customerInfo.SyncLiveCasino;
            metaModel.IsAllowKeno = ProductAccess.IsAllowKN;
            metaModel.IsAllowAgCasino = ProductAccess.IsAllowAG;
            metaModel.IsAllowGoldDeluxe = ProductAccess.IsAllowGD;
            metaModel.IsAllowMacau = ProductAccess.IsAllowMC;
            metaModel.IsAllowMicroGamingLiveCasino = ProductAccess.IsAllowMGCLC;
            metaModel.IsAllowMicroGamingRNGCasino = ProductAccess.IsAllowMGCRNG;
            metaModel.IsAllowBetTrade = ProductAccess.IsAllowBT;
            ViewBag.HasCustComm = licenseeSession.IsAllowedCustomerComm;

            viewModel.Param = metaModel;
            return viewModel;
        }

        /// <summary>
        /// Get bet setting view model.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <param name="custId">The customer ID.</param>
        /// <param name="customerInfo">Customer info.</param>
        /// <returns>Bet setting view model.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private BetSettingViewModel GetViewModel(int productId, int custId, CustomerInfo customerInfo)
        {
            BetSettingViewModel model;
            switch (productId)
            {
                case ProductTypes.Sportsbook:
                    model = _betSettingService.GetSportBookViewModel(custId);
                    break;

                case ProductTypes.Racing:
                    var isAllowSaToteCurrency = ProductService.IsAllowedSaToteCurrency(custId);
                    model = _betSettingService.GetRacingViewModel(custId, isAllowSaToteCurrency);
                    break;

                case ProductTypes.NumberGame:
                    model = _betSettingService.GetNumberGameViewModel(custId);
                    break;

                case ProductTypes.VirtualSports:
                    model = _betSettingService.GetVirtualSportsViewModel(custId);
                    break;

                case ProductTypes.LiveCasino:
                    model = _betSettingService.GetLiveCasinoViewModel(custId, SessionService.UserSession.Language);
                    model.IsEnabled = IsLiveCasinoEnabled(customerInfo);
                    break;

                case ProductTypes.RngCasino:
                    model = GetRngCasinoViewModel(custId, customerInfo, SessionService.UserSession.Language);
                    break;

                case ProductTypes.Keno:
                    model = _betSettingService.GetKenoViewModel(custId);
                    break;

                case ProductTypes.AgCasino:
                    model = _betSettingService.GetAgCasinoViewModel(custId, SessionService.LicenseeSession.LicenseeName);
                    break;

                case ProductTypes.GoldDeluxe:
                    model = _betSettingService.GetGoldDeluxeViewModel(custId, SessionService.LicenseeSession.LicenseeName);
                    break;

                case ProductTypes.Macau:
                case ProductTypes.MicroGamingLiveCasino:
                case ProductTypes.MicrogamingRNGCasino:
                case ProductTypes.BetTrade:
                    model = _betSettingService.GetThirdPartyViewModel(custId, productId);
                    break;

                default:
                    throw new ArgumentException("Invalid product id.");
            }

            return model;
        }

        private RngCasinoBetSettingViewModel GetRngCasinoViewModel(int custId, CustomerInfo customerInfo, string language)
        {
            IList<GameKind> gameKinds;
            if (customerInfo.SyncCasino)
            {
                gameKinds = _casinoSetting.GetRngCasinoSettings(custId, SessionService.LicenseeSession.LicenseeName, language);
            }
            else
            {
                gameKinds = new List<GameKind>();
            }

            return new RngCasinoBetSettingViewModel
            {
                GameKinds = gameKinds,
                IsEnabled = IsRngCasinoEnabled(customerInfo)
            };
        }

        #endregion Common
    }
}