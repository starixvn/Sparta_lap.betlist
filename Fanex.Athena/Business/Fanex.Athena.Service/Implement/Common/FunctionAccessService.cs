// -----------------------------------------------------------------------
// <copyright file="FunctionAccessService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// Function access service class.
    /// </summary>
    public class FunctionAccessService : IFunctionAccessService
    {
        private readonly ISessionService _sessionService;

        public FunctionAccessService()
        {
            _sessionService = new SessionService();
        }

        #region Property

        public bool CanViewTotalBets
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.TotalBets); }
        }

        public bool CanViewForecasts
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.Forecast); }
        }

        public bool CanViewReports
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.Reports); }
        }

        public bool CanViewDataWarehouse
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.DataWarehouse); }
        }

        public bool CanManageMember
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.CustomerList); }
        }

        public bool CanManageAccount
        {
            get { return _sessionService.UserSession.FunctionIdList.Contains((int)FunctionType.AccountManagement); }
        }

        #endregion Property
    }
}