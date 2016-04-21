// --------------------------------------------------------------------------
// <copyright file="CustomerLogViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Model.Customer.ViewModel.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// CustomerLogViewModel class.
    /// </summary>
    public class CustomerLogViewModel
    {
        public CustomerLogMetaModel Param { get; set; }

        public IList<CustomerLog> MainData { get; set; }
    }
}