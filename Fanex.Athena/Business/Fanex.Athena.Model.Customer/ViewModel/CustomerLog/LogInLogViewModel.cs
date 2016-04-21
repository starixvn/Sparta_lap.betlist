// --------------------------------------------------------------------------
// <copyright file="LogInLogViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Models.ViewModel.Customer.CustomerLog
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