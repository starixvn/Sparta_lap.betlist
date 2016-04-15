// -----------------------------------------------------------------------
// <copyright file="IBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    /// <summary>
    ///     IBet setting service.
    /// </summary>
    public interface IBetSettingService
    {
        #region Get Bet Setting

        ThirdPartyBetSettingViewModel GetThirdPartyViewModel(int custId, int productid);

        GoldDeluxeBetSettingViewModel GetGoldDeluxeViewModel(int custId, string siteName);

        AgCasinoBetSettingViewModel GetAgCasinoViewModel(int custId, string licenseeName);

        IList<AgCasinoGame> GetAgCasinoGroupBettings(int custId, string siteName, int currency);

        BetSettingViewModel GetKenoViewModel(int custId);

        BetSettingViewModel GetSportBookViewModel(int custId);

        BetSettingViewModel GetRacingViewModel(int custId, bool isAllowSaToreCurrency);

        BetSettingViewModel GetVirtualSportsViewModel(int custId);

        LiveCasinoBetSettingViewModel GetLiveCasinoViewModel(int custId, string language);

        #endregion Get Bet Setting

        #region Update Bet Setting

        IResultInfo UpdateGoldDeluxeBetSetting(string siteName, int custId, string group);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingParam);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, IList<VSBetSetting> betSettings);

        IResultInfo UpdateRngCasinoSetting(RngCasinoBetSettingMetaModel metaModel);

        IResultInfo UpdateLiveCasinoBetSetting(
            int userId,
            int custId,
            List<LiveCasinoBetSetting> metaModel,
            string ip,
           string language);

        IResultInfo UpdateGoldDeluxeStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip);

        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip, string username);

        IResultInfo UpdateThirdPartyProductStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled);

        #endregion Update Bet Setting
    }
}