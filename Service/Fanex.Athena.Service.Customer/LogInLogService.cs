// -----------------------------------------------------------------------
// <copyright file="LogInLogService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Service.Customer.Interfaces;

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