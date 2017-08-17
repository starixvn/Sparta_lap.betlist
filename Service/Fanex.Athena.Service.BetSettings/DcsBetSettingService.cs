// -----------------------------------------------------------------------
// <copyright file="DcsBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Text;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using Resources;

    public class DcsBetSettingService : BetSettingBaseService, IDcsBetSettingService
    {
        private readonly IDcsBetSettingDataAccess _betSettingDataAccess;

        public DcsBetSettingService()
        {
            _betSettingDataAccess = new DcsBetSettingDataAccess();
        }

        public DcsBetSettingService(IDcsBetSettingDataAccess betSettingDataAccess, IAuthorizationService authorizationService)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
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
                    Message = ViewResources.AccessDeniedMsg
                };
            }

            return _betSettingDataAccess.UpdateStatus(userId, custId, ip, status, productId);
        }

        public IList<BetSetting> GetBetSettings(int licenseeId, int custId, int productId)
        {
            return HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId, productId)
                        : new List<BetSetting>();
        }

        public IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string ip, IEnumerable<DcsBetSetting> betSettings)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            const string XMLElement = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" ></BetLimit>";
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            foreach (var setting in betSettings)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, setting.SportId, setting.BetType, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));
            }

            var xmlparam = string.Format(XMLPattern, xmlElementBuilder.ToString());

            return _betSettingDataAccess.UpdateBetSetting(
                custId,
                userId,
                ip,
                xmlparam);
        }
    }
}