// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System;
    #region Using

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetList.DTO;
    using NPOI.SS.UserModel;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter.Interfaces;
    using Web.ResultExtension;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportCancelledBets)]
    public class CancelledBetsBetListController : BetListBaseController
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ICancelledBetListDataAccess _cancelledBetListDataAccess;

#pragma warning disable S107 // Methods should not have too many parameters
        public CancelledBetsBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IAuthorizationService authorizationService,
            ICancelledBetListDataAccess cancelledBetListDataAccess)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
            _authorizationService = authorizationService;
            _cancelledBetListDataAccess = cancelledBetListDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewParam = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(viewParam);

            var model = CreateViewModel(viewParam);

            return View("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(BetListExcelMetaModel metaModel)
        {
            if (metaModel.IsExportExcel)
            {
                return Excel(metaModel);
            }

            var viewParam = CreateDefaultMetaModel(metaModel);

            var model = CreateViewModel(viewParam);

            return View("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml", model);
        }

        private CancelledBetsBetListViewModel CreateViewModel(BetListMetaModel viewParam)
        {
            viewParam.AgentListXml = SessionService.GenerateAgentListXml(viewParam.CurrencyId);
            var customers = GetData(viewParam);

            if (customers.Any())
            {
                viewParam.CustId = customers.First().CustId;
            }

            var userSession = SessionService.UserSession;
            return new CancelledBetsBetListViewModel
            {
                ShowEmptyContent = true,
                Param = viewParam,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(viewParam.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                CustomerList = customers
            };
        }

        private ActionResult Excel(BetListExcelMetaModel metaModel)
        {
            if (metaModel.CurrencyId == BaseCurrencyType.All ||
                SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_SelDeleted";
                CreateDefaultMetaModel(metaModel);
                metaModel.UspKey = UspKey;
                
                var currencyTitle = GetViewInCurrency(metaModel);
                metaModel.ShowingOption = new ShowingOption
                {
                    CustomerCommVisible = metaModel.HasPlayerCom
                };
                metaModel.HiddenColumns = HiddenColumnIndexes;
                metaModel.Title = $"{ViewResources.CancelledBets} - {currencyTitle}";
                return ProcessExportExcel(metaModel, "CancelledBetList");
            }

            return EmptyExcel();
        }

        private IEnumerable<CustomerCancelBetItem> GetData(BetListMetaModel metaModel)
        {
            return HasPermissionOnData(metaModel) ?
                _cancelledBetListDataAccess.GetCancelBetCustList(metaModel) :
                new List<CustomerCancelBetItem>();
        }

        private bool HasPermissionOnData(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId);
        }
    }
}