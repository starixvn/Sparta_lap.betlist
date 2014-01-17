// --------------------------------------------------------------------------
// <copyright file="LogInLogDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Implement.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Interface.Customer.CustomerLog;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Login log data access.
    /// </summary>
    public class LogInLogDataAccess : ILogInLogDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="username">Username value.</param>
        /// <returns>The result value.</returns>
        public IList<LogInLog> GetDataList(int licenseeId, string username)
        {
            const string SpName = "Lrf_UserLog_LoginSel";

            var dataList = new List<LogInLog>();

            using (var db = new ObjectDb(SpName))
            {
                db.AddInParameter("UserName", DbType.String, string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
                db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);

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