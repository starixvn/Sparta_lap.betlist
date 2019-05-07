// --------------------------------------------------------------------------
// <copyright file="LogInLogDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Common.Extensions;

    public class LogInLogDataAccess : BaseDataAccess, ILogInLogDataAccess
    {
        public async Task<IList<LogInLog>> GetDataList(int licenseeId, string username, string agentListXml)
        {
            const string SpName = "Lrf_UserLog_LoginSel";

            var dataList = new List<LogInLog>();

            using (var db = this.GetObjectDb(SpName))
            {
                db.SetParameterValue("UserName", string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
                db.SetParameterValue("LicenseeId", licenseeId);
                db.SetParameterValue("AgentList", agentListXml);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var log = new LogInLog
                        {
                            UserName = reader.ToString("UserName"),
                            CreatedDate = reader.ToDateTime2("CreatedDate"),
                            LastTxn = reader.ToDateTime2("LastTxn")
                        };
                        dataList.Add(log);
                    }
                }

                return dataList;
            }
        }
    }
}