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

        private readonly LogInLogDataAccess _logInLogDataAccess;

        #endregion Field

        #region Constructor

        public LogInLogService()
        {
            _logInLogDataAccess = new LogInLogDataAccess();
        }

        #endregion Constructor

        public IList<LogInLog> GetDataList(int licenseeId, string username)
        {
            return this._logInLogDataAccess.GetDataList(licenseeId, username);
        }
    }
}