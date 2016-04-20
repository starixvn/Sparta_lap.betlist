// -----------------------------------------------------------------------
// <copyright file="BetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    #region

    using System.Collections.Generic;
    using System.Text;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetSettings;
    using Fanex.Athena.Service.BetSettings.Interfaces;

    #endregion

    /// <summary>
    ///     Bet setting service.
    /// </summary>
    public class BetSettingService : BetSettingBaseService, IBetSettingService
    {
        private readonly IBetSettingDataAccess _betSettingDataAccess;

        public BetSettingService()
        {
            _betSettingDataAccess = new BetSettingDataAccess();
        }

        public BetSettingViewModel GetSportBookViewModel(int custId)
        {
            var esportSetting = EsportSettingType.NoneOffering;
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetSportBookBetSetting(custId, ref esportSetting)
                        : new List<BetSetting>();

            return new BetSettingViewModel
            {
                MainData = betSettings,
                EsportSetting = esportSetting
            };
        }

        public BetSettingViewModel GetVirtualSportsViewModel(int custId)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetVirtualSportsBetSetting(custId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        public IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            if (!HasPermissionOnData(setting.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateSportsBookBetSetting(setting, hashParam);
        }

        public IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            return _betSettingDataAccess.UpdateEsportStatus(setting, isDisabled);
        }

        public IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, IList<VSBetSetting> betSettings)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting sporttype=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            foreach (var setting in betSettings)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, setting.SportId, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));
            }

            var xmlparam = string.Format(XMLPattern, xmlElementBuilder.ToString());

            return _betSettingDataAccess.UpdateVirtualSportsBetSetting(
                custId,
                userId,
                language,
                ip,
                xmlparam);
        }
    }
}