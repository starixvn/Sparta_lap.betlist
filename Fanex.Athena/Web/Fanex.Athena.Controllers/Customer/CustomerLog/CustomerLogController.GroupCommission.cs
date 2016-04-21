// --------------------------------------------------------------------------
// <copyright file="CustomerLogController.GroupCommission.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;
    using Fanex.Athena.Models.ViewModel.Customer.CustomerLog;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Group commission log.
    /// </summary>
    public partial class CustomerLogController
    {
        /// <summary>
        /// Group commission action.
        /// </summary>
        /// <returns>Group commission log view.</returns>
        [FunctionAuthorization(ParentUrl = CustomerLogUrl)]
        public ActionResult GroupCommission()
        {
            var model = new CustomerLogViewModel { Param = new CustomerLogMetaModel(), MainData = new List<CustomerLog>() };

            return View("~/Views/Customer/CustomerLog/GroupCommLog/Main.cshtml", model);
        }
    }
}