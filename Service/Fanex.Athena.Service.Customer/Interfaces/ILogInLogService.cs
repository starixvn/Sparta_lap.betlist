// --------------------------------------------------------------------------
// <copyright file="ILogInLogService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;

    public interface ILogInLogService
    {
        IList<LogInLog> GetDataList(int licenseeId, string username, string agentListXml);
    }
}