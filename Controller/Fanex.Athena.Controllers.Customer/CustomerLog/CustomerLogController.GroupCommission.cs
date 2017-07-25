// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.GroupCommission.cs" company="">
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
        public ActionResult GroupCommission()
        {
            var model = new CustomerLogViewModel { Param = new CustomerLogMetaModel(), MainData = new List<CustomerLog>() };

            return View("~/Views/Customer/CustomerLog/GroupCommLog/Main.cshtml", model);
        }
    }
}