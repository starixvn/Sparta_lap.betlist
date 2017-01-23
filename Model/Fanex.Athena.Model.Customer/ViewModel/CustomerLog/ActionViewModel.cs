// --------------------------------------------------------------------------
// <copyright file="ActionViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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