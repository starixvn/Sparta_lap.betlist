// --------------------------------------------------------------------------
// <copyright file="DcsBetSettingController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DataDomain;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;

    public class DcsBetSettingController : BetSettingBaseController
    {
        private static readonly IDictionary<int, string> ProductViewNameMap = new Dictionary<int, string>
        {
            { ProductTypes.AllBet, "AllBet" },
            { ProductTypes.Voidbridge, "Voidbridge" },
            { ProductTypes.FishingWorld, "FishingWorld" },
            { ProductTypes.VirtualSports, "VirtualSports" },
            { ProductTypes.VirtualSports2, "VirtualSports2" }
        };

        private static readonly IEnumerable<int> ProductsHasStatus = new int[]
        {
             ProductTypes.AllBet, ProductTypes.Voidbridge, ProductTypes.FishingWorld
        };

        private static readonly IEnumerable<int> ProductsHasBetLimit = new int[]
        {
             ProductTypes.VirtualSports, ProductTypes.VirtualSports2
        };

        private static readonly IDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
        {
            [ProductTypes.VirtualSports] = new string[] { "180", "181", "182", "183", "184", "185", "186" },
            [ProductTypes.VirtualSports2] = new string[] { "190", "191", "199" }
        };

        private readonly IDcsBetSettingService _betSettingService;

        public DcsBetSettingController()
        {
            _betSettingService = new DcsBetSettingService();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(BetSettingMetaModel metaModel)
        {
            if (!ProductViewNameMap.ContainsKey(metaModel.ProductId))
            {
                throw new ArgumentException("Invalid product id: " + metaModel.ProductId);
            }

            var viewModel = new BetSettingViewModel();
            AssignRequiredProperties(viewModel, metaModel);

            var viewName = ProductViewNameMap[metaModel.ProductId];
            var viewPath = $"~/Views/Customer/BetSetting/{viewName}.cshtml";
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            if (ProductsHasStatus.Contains(metaModel.ProductId))
            {
                viewModel.IsEnabled = _betSettingService.GetStatus(licenseeId, metaModel.CustId, metaModel.ProductId);
            }

            if (ProductsHasBetLimit.Contains(metaModel.ProductId))
            {
                viewModel.MainData = _betSettingService.GetBetSettings(licenseeId, metaModel.CustId, metaModel.ProductId);
            }

            return View(viewPath, viewModel);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateStatus(int custId, string nickName, int status, int productId)
        {
            if (!ProductAccess.IsAllow(productId) || !IsBetSettingEditable())
            {
                return Json(new ResultInfo { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var userSession = SessionService.UserSession;
            var result = _betSettingService.UpdateStatus(
                SessionService.LicenseeSession.LicenseeId,
                userSession.UserId,
                custId,
                Request.UserHostAddress,
                status == 1,
                productId);

            if (result.Code == 0)
            {
                result.Message = string.Format(ViewResources.UpdateSuccess, nickName);
            }

            return Json(result);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult UpdateBetSetting(int productId, string nickName)
        {
            if (!ProductAccess.IsAllow(productId) || !IsBetSettingEditable())
            {
                return Json(new ResultInfo { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            var commonParam = GetBetSettingCommonParam();
            var parameters = GetParameters(productId);

            if (parameters.Count == 0 || commonParam.UserId == 0 || commonParam.CustId == 0)
            {
                return Json(new ResultInfo { Code = 0, Message = ViewResources.InvalidParams }, JsonRequestBehavior.AllowGet);
            }

            var result = _betSettingService.UpdateBetSetting(
                commonParam.LicenseeId,
                commonParam.CustId,
                commonParam.UserId,
                commonParam.Ip,
                parameters);

            if (result.Code == 0)
            {
                result.Message = string.Format(productId == ProductTypes.VirtualSports
                    ? ViewResources.UpdateVirtualSportsBetSettingSuccessMsg
                    : ViewResources.UpdateVirtualSports2BetSettingSuccessMsg,
                    nickName);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IList<DcsBetSetting> GetParameters(int productId)
        {
            if (!ProductSportList.ContainsKey(productId))
            {
                return new List<DcsBetSetting>();
            }

            var sports = ProductSportList[productId];
            var settings = new List<DcsBetSetting>();

            foreach (var sport in sports)
            {
                settings.Add(new DcsBetSetting
                {
                    MaxBet = GetDecimalRequest("maxbet" + sport, 0),
                    MinBet = GetDecimalRequest("minbet" + sport, 0),
                    MaxPerMatch = GetDecimalRequest("maxpermatch" + sport, 0),
                    SportId = sport,
                    BetType = GetIntRequest("bettype" + sport, 0),
                });
            }

            return settings;
        }
    }
}