// --------------------------------------------------------------------------
// <copyright file="LogInLog.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.DTO.CustomerLog
{
    using System;

    /// <summary>
    /// LogInLog class.
    /// </summary>
    public class LogInLog
    {
        public int LogId { get; set; }

        public string UserName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastLogIn { get; set; }

        public DateTime? LastTxn { get; set; }
    }
}