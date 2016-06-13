// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.LogInLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

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
        /// <summary>
        /// Log in log action.
        /// </summary>
        /// <returns>Log in log view.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult LogInLog()
        {
            var model = new LogInLogViewModel { Param = new CustomerLogMetaModel(), MainData = new List<LogInLog>() };

            return View("~/Views/Customer/CustomerLog/LogInLog/Main.cshtml", model);
        }
    }
}