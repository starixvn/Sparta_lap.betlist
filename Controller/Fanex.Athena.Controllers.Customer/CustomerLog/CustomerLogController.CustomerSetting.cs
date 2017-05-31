// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.CustomerSetting.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;
    using Fanex.Athena.Model.Customer.ViewModel.CustomerLog;

    public partial class CustomerLogController
    {
        [HttpGet]
        public ActionResult CustomerSetting()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productOption = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language);

            var model = new CustomerLogViewModel
            {
                Param = new CustomerLogMetaModel(),
                MainData = new List<CustomerLog>()
            };

            model.Param.IsAllowBA = productOption.IsAllowBA;
            model.Param.IsAllowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Customer/CustomerLog/CustomerSettingLog/Main.cshtml", model);
        }
    }
}