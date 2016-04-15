// -----------------------------------------------------------------------
// <copyright file="VoidbridgeBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.BetSettings
{
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Interface.BetSettings;
    using Models.DTO.Common;
    using Models.ViewModel;

    public class VoidbridgeBetSettingService : BetSettingBaseService, IVoidbridgeBetSettingService
    {
        private readonly IVoidbridgeBetSettingDataAccess _voidbridgeDataAccess;

        public VoidbridgeBetSettingService()
        {
            _voidbridgeDataAccess = new VoidbridgeBetSettingDataAccess();
        }

        public bool? GetStatus(int custId)
        {
            if (!HasPermissionOnData(custId))
            {
                return null;
            }

            return _voidbridgeDataAccess.GetStatus(custId);
        }

        public IResultInfo UpdateStatus(int userId, int custId, string ip, bool status)
        {
            if (!HasPermissionOnData(custId))
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