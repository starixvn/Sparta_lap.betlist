// --------------------------------------------------------------------------
// <copyright file="ILogInLogService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;

    /// <summary>
    /// Login log interface.
    /// </summary>
    public interface ILogInLogService
    {
        IList<LogInLog> GetDataList(int licenseeId, string username);
    }
}