// --------------------------------------------------------------------------
// <copyright file="LogInLogViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.ViewModel.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// LogInLogViewModel class.
    /// </summary>
    public class LogInLogViewModel
    {
        public CustomerLogMetaModel Param { get; set; }

        public IList<LogInLog> MainData { get; set; }
    }
}