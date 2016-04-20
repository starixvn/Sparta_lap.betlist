// --------------------------------------------------------------------------
// <copyright file="ILogInLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;

    /// <summary>
    /// Login log interface.
    /// </summary>
    public interface ILogInLogService
    {
        IList<LogInLog> GetDataList(int licenseeId, string username);
    }
}