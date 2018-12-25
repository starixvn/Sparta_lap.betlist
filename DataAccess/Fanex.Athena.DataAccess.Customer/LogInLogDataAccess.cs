// --------------------------------------------------------------------------
// <copyright file="LogInLogDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Common.Extensions;

    public class LogInLogDataAccess : BaseDataAccess, ILogInLogDataAccess
    {
        public IList<LogInLog> GetDataList(int licenseeId, string username, string agentListXml)
        {
            const string SpName = "Lrf_UserLog_LoginSel";

            var dataList = new List<LogInLog>();

            using (var db = this.GetObjectDb(SpName))
            {
                db.AddInParameter("UserName", DbType.String, string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
                db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                db.AddInParameter("AgentList", DbType.String, agentListXml);
                using (var reader = db.ExecuteReader())
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