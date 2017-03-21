// --------------------------------------------------------------------------
// <copyright file="ILogInLogDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;

    public interface ILogInLogDataAccess
    {
        IList<LogInLog> GetDataList(int licenseeId, string username, string agentListXml);
    }
}