// --------------------------------------------------------------------------
// <copyright file="ActionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Model.Customer.ViewModel.CustomerLog
{
    using System.Collections.Generic;

    /// <summary>
    /// ActionViewModel class.
    /// </summary>
    public class ActionViewModel
    {
        public IDictionary<string, string> Data { get; set; }

        public IDictionary<string, string> Resource { get; set; }
    }
}