// -----------------------------------------------------------------------
// <copyright file="VoidbridgeBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.BetSettings
{
    using DataAccess.Implement.BetSettings;
    using DataAccess.Interface.BetSettings;
    using Interface.Users;
    using Models.DTO.Common;
    using Models.ViewModel;
    using Users;

    public class VoidbridgeBetSettingService : IVoidbridgeBetSettingService
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly IVoidbridgeBetSettingDataAccess _voidbridgeDataAccess;

        public VoidbridgeBetSettingService()
        {
            _authorizationService = new AuthorizationService();
            _voidbridgeDataAccess = new VoidbridgeBetSettingDataAccess();
        }

        public bool? GetStatus(int custId)
        {
            if (!_authorizationService.HasPermissionOnData(custId, 0))
            {
                return null;
            }

            return _voidbridgeDataAccess.GetStatus(custId);
        }

        public IResultInfo UpdateStatus(int userId, int custId, string ip, bool status)
        {
            if (!_authorizationService.HasPermissionOnData(custId, 0))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }
            
            return _voidbridgeDataAccess.UpdateStatus(userId, custId, ip, status);
        }
    }
}