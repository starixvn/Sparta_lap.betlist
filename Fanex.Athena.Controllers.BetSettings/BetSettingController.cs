// -----------------------------------------------------------------------
// <copyright file="BetSettingController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.BetSettings;
    using Fanex.Athena.Models.ViewModel.BetSettings;
    using Fanex.Athena.Web.ActionFilter;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class BetSettingController : BetSettingBaseController
    {
        private readonly IBetSettingService _betSettingService;

        public BetSettingController()
        {
            this._betSettingService = new BetSettingService();
        }

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
                case ProductTypes.Keno:
                    return View("~/Views/Customer/BetSetting/Keno.cshtml", viewModel);

                default: // Sportsbook, Virtual Sports
                    return View("~/Views/Customer/BetSetting/Main.cshtml", viewModel);
            }
        }

        private BetSettingViewModel GetViewModels(BetSettingMetaModel metaModel)
        {
            var customerInfo = CustomerService.GetCustomerInfo(metaModel.CustId);
            var viewModel = GetViewModel(metaModel.ProductId, metaModel.CustId, customerInfo);
            if (viewModel == null)
            {
                return null;
            }

            AssignRequiredProperties(viewModel, metaModel);

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

                case ProductTypes.VirtualSports:
                    model = _betSettingService.GetVirtualSportsViewModel(custId);
                    break;

                case ProductTypes.Keno:
                    model = _betSettingService.GetKenoViewModel(custId);
                    break;

                default:
                    throw new ArgumentException("Invalid product id.");
            }

            return model;
        }
    }
}