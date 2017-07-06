// -----------------------------------------------------------------------
// <copyright file="VirtualSportsBetSettingService.cs" company="">
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
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class VirtualSportsBetSettingService : BetSettingBaseService, IVirtualSportsBetSettingService
    {
        private readonly IVirtualSportsBetSettingDataAccess _betSettingDataAccess;

        public VirtualSportsBetSettingService()
        {
            _betSettingDataAccess = new VirtualSportsBetSettingDataAccess();
        }

        public VirtualSportsBetSettingService(
            IAuthorizationService authorizationService,
            IVirtualSportsBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public BetSettingViewModel GetViewModel(int licenseeId, int custId, int productId)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId, productId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        public IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string ip, IEnumerable<VSBetSetting> betSettings)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            const string XMLElement = "<BetLimit SportType=\"{0}\" MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerMatch=\"{3}\" ></BetLimit>";
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            foreach (var setting in betSettings)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, setting.SportId, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));
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