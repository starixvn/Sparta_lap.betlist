// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.LogInLog.cs" company="">
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
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Log in log controller.
    /// </summary>
    public partial class CustomerLogController
    {
        [HttpGet]
        public ActionResult LogInLog()
        {
            var model = new LogInLogViewModel { Param = new CustomerLogMetaModel(), MainData = new List<LogInLog>() };

            return View("~/Views/Customer/CustomerLog/LogInLog/Main.cshtml", model);
        }
    }
}