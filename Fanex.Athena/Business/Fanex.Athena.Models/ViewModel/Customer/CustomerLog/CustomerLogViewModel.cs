// --------------------------------------------------------------------------
// <copyright file="CustomerLogViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Models.ViewModel.Customer.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;

    /// <summary>
    /// CustomerLogViewModel class.
    /// </summary>
    public class CustomerLogViewModel
    {
        public CustomerLogMetaModel Param { get; set; }

        public IList<CustomerLog> MainData { get; set; }
    }
}