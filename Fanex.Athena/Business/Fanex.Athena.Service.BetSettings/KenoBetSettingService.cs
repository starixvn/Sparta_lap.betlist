// -----------------------------------------------------------------------
// <copyright file="KenoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Text;
    using DataAccess.BetSettings;
    using Models.DTO.BetSettings;
    using Models.DTO.Common;
    using Models.ViewModel;
    using Models.ViewModel.BetSettings;

    public class KenoBetSettingService : BetSettingBaseService, IKenoBetSettingService
    {
        private readonly IKenoBetSettingDataAccess _betSettingDataAccess;

        public KenoBetSettingService()
        {
            _betSettingDataAccess = new KenoBetSettingDataAccess();
        }

        public BetSettingViewModel GetViewModel(int custId)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetBetSettings(custId)
                        : new List<BetSetting>();

            return new BetSettingViewModel()
            {
                MainData = betSettings
            };
        }

        public IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting GameTypeID=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(custId))
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