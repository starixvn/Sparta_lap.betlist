// --------------------------------------------------------------------------
// <copyright file="ILogInLogService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

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