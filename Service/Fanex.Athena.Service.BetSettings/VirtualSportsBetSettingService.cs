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

        public BetSettingViewModel GetViewModel(int licenseeId, int custId)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        public IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string language, string ip, IEnumerable<VSBetSetting> betSettings)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting sporttype=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
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
                language,
                ip,
                xmlparam);
        }
    }
}