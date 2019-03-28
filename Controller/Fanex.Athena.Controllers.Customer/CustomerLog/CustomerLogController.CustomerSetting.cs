// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.CustomerSetting.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;
    using Fanex.Athena.Model.Customer.ViewModel.CustomerLog;

    public partial class CustomerLogController
    {
        [HttpGet]
        public async Task<ActionResult> CustomerSetting()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var productOption = await _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language).ConfigureAwait(false);

            var model = new CustomerLogViewModel
            {
                Param = new CustomerLogMetaModel(),
                MainData = new List<CustomerLog>()
            };

            model.Param.IsAllowBA = productOption.IsAllowBA;
            model.Param.IsAllowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Desktop/Customer/CustomerLog/CustomerSettingLog/Main.cshtml", model);
        }
    }
}