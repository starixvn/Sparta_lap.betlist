// -----------------------------------------------------------------------
// <copyright file="KenoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Text;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class KenoBetSettingService : BetSettingBaseService, IKenoBetSettingService
    {
        private readonly IKenoBetSettingDataAccess _betSettingDataAccess;

        public KenoBetSettingService()
        {
            _betSettingDataAccess = new KenoBetSettingDataAccess();
        }

        public KenoBetSettingService(
            IAuthorizationService authorizationService,
            IKenoBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public BetSettingViewModel GetViewModel(int licenseeId, int custId)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSettings(custId)
                        : new List<BetSetting>();

            return new BetSettingViewModel()
            {
                MainData = betSettings
            };
        }

        public IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string language, string ip, KenoBetSetting setting)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting GameTypeID=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            xmlElementBuilder.Append(string.Format(XMLElement, setting.GameTypeID, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));

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