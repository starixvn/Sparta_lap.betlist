// -----------------------------------------------------------------------
// <copyright file="VoidbridgeBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using BetSettings.Interfaces;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Model.Core.DTO.Common;
    using Resources;

    public class VoidbridgeBetSettingService : BetSettingBaseService, IVoidbridgeBetSettingService
    {
        private readonly IVoidbridgeBetSettingDataAccess _betSettingDataAccess;

        public VoidbridgeBetSettingService()
        {
            _betSettingDataAccess = new VoidbridgeBetSettingDataAccess();
        }

        public bool? GetStatus(int licenseeId, int custId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return null;
            }

            return _betSettingDataAccess.GetStatus(custId);
        }

        public IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string ip, bool status)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            return _betSettingDataAccess.UpdateStatus(userId, custId, ip, status);
        }
    }
}