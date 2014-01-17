// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.CustomerSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.25">Created</change>
// </history>

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;
    using Fanex.Athena.Models.ViewModel.Customer.CustomerLog;

    /// <summary>
    /// Customer setting log.
    /// </summary>
    public partial class CustomerLogController
    {
        /// <summary>
        /// Customer setting action.
        /// </summary>
        /// <returns>Customer setting log view.</returns>
        public ActionResult CustomerSetting()
        {
            var productOption = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);

            var model = new CustomerLogViewModel
            {
                Param = new CustomerLogMetaModel(),
                MainData = new List<CustomerLog>()
            };

            model.Param.IsAllowBA = productOption.IsAllowBa;
            model.Param.IsAllowCustComm = SessionService.LicenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Customer/CustomerLog/CustomerSettingLog/Main.cshtml", model);
        }
    }
}