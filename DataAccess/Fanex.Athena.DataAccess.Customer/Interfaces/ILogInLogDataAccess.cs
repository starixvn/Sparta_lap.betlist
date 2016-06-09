// --------------------------------------------------------------------------
// <copyright file="ILogInLogDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;

    /// <summary>
    /// Login log interface.
    /// </summary>
    public interface ILogInLogDataAccess
    {
        IList<LogInLog> GetDataList(int licenseeId, string username);
    }
}