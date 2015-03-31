// -----------------------------------------------------------------------
// <copyright file="IBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Customer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.Voidbridge;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Customer.BetSetting;

    /// <summary>
    ///     IBet setting service.
    /// </summary>
    public interface IBetSettingService
    {
        VoidbridgeBetSetting GetVoidbridgeSetting(int custId, string siteName);

        GoldDeluxeBetSettingViewModel GetGoldDeluxeSetting(int custId, string siteName);

        AgCasinoBetSetting GetAgCasinoSetting(int custId);

        IList<AgCasinoGame> GetAgCasinoGroupBettingLimit(int custId, string siteName, int currency);

        IList<BetSetting> GetKenoBetSetting(int custId);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting);

        IList<BetSetting> GetNumberGameBetSetting(int custId);

        IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        IList<LiveCasinoBetSettingViewModel> GetLiveCasinoBetSetting(int custId, string language);

        IResultInfo UpdateGoldDeluxeBetSetting(string siteName, int custId, string group);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingParam);

        IResultInfo UpdateNumberGameBetSetting(CommonBetSetting setting, Hashtable data);

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

        IResultInfo UpdateVoidBridgeStatus(int userId, int custId, string language, string ip, bool isEnabled);
    }
}