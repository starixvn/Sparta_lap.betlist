// -----------------------------------------------------------------------
// <copyright file="DcsBetSettingService.cs" company="">
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

    public class DcsBetSettingService : BetSettingBaseService, IDcsBetSettingService
    {
        private readonly IDcsBetSettingDataAccess _betSettingDataAccess;

        public DcsBetSettingService()
        {
            _betSettingDataAccess = new DcsBetSettingDataAccess();
        }

        public bool? GetStatus(int licenseeId, int custId, int productId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return null;
            }

            return _betSettingDataAccess.GetStatus(custId, productId);
        }

        public IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string ip, bool status, int productId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            return _betSettingDataAccess.UpdateStatus(userId, custId, ip, status, productId);
        }
    }
}