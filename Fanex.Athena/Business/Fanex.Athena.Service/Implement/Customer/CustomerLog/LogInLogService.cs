// -----------------------------------------------------------------------
// <copyright file="LogInLogService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Customer.CustomerLog
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Customer.CustomerLog;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Service.Interface.Customer.CustomerLog;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogInLogService : ILogInLogService
    {
        #region Field

        protected readonly LogInLogDataAccess LogInLogDataAccess;

        #endregion Field

        #region Constructor

        public LogInLogService()
        {
            LogInLogDataAccess = new LogInLogDataAccess();
        }

        #endregion Constructor

        public IList<LogInLog> GetDataList(int licenseeId, string username)
        {
            return this.LogInLogDataAccess.GetDataList(licenseeId, username);
        }
    }
}