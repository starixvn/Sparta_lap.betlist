// -----------------------------------------------------------------------
// <copyright file="BetSettingController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;
    using Fanex.Athena.Models.MetaModel.Customer.BetSetting;
    using Fanex.Athena.Models.ViewModel.Customer.BetSetting;
    using Fanex.Athena.Service.Implement.Casino;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Casino;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    ///     Bet setting controller.
    /// </summary>
    public class BetSettingController : ReadonlyBaseController
    {
        #region Const

        private const int BopingResellerId = 7;
        private const int Licensee12Bet = 1;
        private const int LicenseeSpondemo = 5;

        #endregion Const

        #region Field

        private readonly IBetSettingService _betSettingService;
        private readonly ILicenseeService _licenseeService;
        private readonly IProductService _productService;
        private readonly ICasinoSetting _casinoSetting;
        private readonly ICustomerService _customerService;

        #endregion Field

        #region Constructor

        public BetSettingController()
        {
            this._betSettingService = new BetSettingService();
            this._productService = new ProductService();
            this._licenseeService = new LicenseeService();
            this._casinoSetting = new CasinoSetting();
            this._customerService = new CustomerService();
        }

        #endregion Constructor

        #region Displaying bet settings

        /// <summary>
        /// The home page.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            var viewModel = GetViewModels(metaModel);

            switch (metaModel.TabId)
            {
                case ProductType.NumberGame:
                    return View("~/Views/Customer/BetSetting/NumberGame.cshtml", viewModel);
                case ProductType.LiveCasino:
                    return View("~/Views/Customer/BetSetting/LiveCasino.cshtml", viewModel);
                case ProductType.RngCasino:
                    return View("~/Views/Customer/BetSetting/RngCasino.cshtml", viewModel);
                default: // Sportsbook, Racing, Virtual Sports
                    return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
            }
        }

        #endregion Displaying bet settings

        #region Common

        /// <summary>
        /// Get view model.
        /// Get list of LicenseeId base on ResellerId.
        /// BOPING's Id = 7.
        /// Set editable. Only 12Bet (licenseeId = 1), SPONDEMO (licenseeId = 5) and BOPING can edit bet setting.
        ///  Checking sync casino.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>Bet setting view model.</returns>
        [NonAction]
        private BetSettingViewModel GetViewModels(BetSettingMetaModel metaModel)
        {
            var licenceeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.LicenessId = licenceeId;
            var userId = SessionService.UserSession.UserId;
            var licenseeList = _licenseeService.GetByResellerId(userId, BopingResellerId);
            metaModel.Editable = licenseeList.Contains(licenceeId) || metaModel.LicenessId == Licensee12Bet ||
                                 metaModel.LicenessId == LicenseeSpondemo;
            var productAccess = this._productService.GetProductAccess(licenceeId, SessionService.UserSession.Language);
            metaModel.IsAllowSportsBook = productAccess.IsAllowSB;
            metaModel.IsAllowRacing = productAccess.IsAllowRb;            
            metaModel.IsAllowVirtualSport = productAccess.IsAllowVs;
            metaModel.IsAllowNumberGame = productAccess.IsAllowNg;
            var customerInfo = _customerService.GetCustomerInfo(metaModel.CustId);            
            metaModel.IsAllowRngCasino = productAccess.IsAllowCs && customerInfo.SyncCasino;
            metaModel.IsAllowLiveCasino = productAccess.IsAllowLc && customerInfo.SyncLiveCasino;
            return GetViewModel(metaModel, customerInfo);
        }

        /// <summary>
        /// Get bet setting view model.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <param name="customerInfo">Customer info.</param>
        /// <returns>Bet setting view model.</returns>
        private BetSettingViewModel GetViewModel(BetSettingMetaModel metaModel, CustomerInfo customerInfo)
        {
            var model = new BetSettingViewModel { Param = metaModel };

            switch (metaModel.TabId)
            {
                case ProductType.Sportsbook:
                    model.MainData = _betSettingService.GetSportBookBetSetting(metaModel.CustId);
                    break;
                case ProductType.Racing:
                    var isAllowSaToteCurrency = _productService.IsAllowedSaToteCurrency(metaModel.CustId);
                    model.MainData = _betSettingService.GetRacingBetSetting(metaModel.CustId, isAllowSaToteCurrency);
                    break;
                case ProductType.NumberGame:
                    model.MainData = _betSettingService.GetNumberGameBetSetting(metaModel.CustId);
                    break;
                case ProductType.VirtualSports:
                    model.MainData = _betSettingService.GetVirtualSportsBetSetting(metaModel.CustId);
                    break;
                case ProductType.LiveCasino:
                    model.LiveCasinoData = _betSettingService.GetLiveCasinoBetSetting(metaModel.CustId);
                    model.IsEnabled = IsLiveCasinoEnabled(customerInfo);
                    break;
                case ProductType.RngCasino:
                    if (customerInfo.SyncCasino)
                    {
                        model.RngCasinoData = _casinoSetting.GetRngCasinoSetting(metaModel.CustId, SessionService.LicenseeSession.LicenseeName);
                    }
                    else
                    {
                        model.RngCasinoData = new RngCasinoBetSetting()
                        {
                            Data = new List<GameKind>()
                        };
                    }

                    model.IsEnabled = IsRngCasinoEnabled(customerInfo);
                    break;
            }

            return model;
        }

        /// <summary>
        /// Check if Live Casino is enabled.
        /// </summary>
        /// <param name="customerInfo">Customer Information.</param>
        /// <returns>Enabled value.</returns>
        private bool? IsLiveCasinoEnabled(CustomerInfo customerInfo)
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

        /// <summary>
        /// Check if RNG Casino is enabled.
        /// </summary>
        /// <param name="customerInfo">Customer Information.</param>
        /// <returns>Enabled value.</returns>
        private bool? IsRngCasinoEnabled(CustomerInfo customerInfo)
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

        #endregion Common
    }
}