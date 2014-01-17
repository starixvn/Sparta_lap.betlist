// --------------------------------------------------------------------------
// <copyright file="ILogInLogDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Interface.Customer.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;

    /// <summary>
    /// Login log interface.
    /// </summary>
    public interface ILogInLogDataAccess
    {
        IList<LogInLog> GetDataList(int licenseeId, string username);
    }
}