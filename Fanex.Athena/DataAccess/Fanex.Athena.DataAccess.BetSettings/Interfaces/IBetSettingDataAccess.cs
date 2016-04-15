// -----------------------------------------------------------------------
// <copyright file="IBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;
    using Fanex.Athena.Models.DTO.Common;

    public interface IBetSettingDataAccess
    {
        AgCasinoBetSetting GetAgCasinoSetting(int custId);

        GoldDeluxeBetSetting GetGoldDeluxeSetting(int custId);

        IList<BetSetting> GetKenoBetSettings(int custId);

        IList<LiveCasinoBetSetting> GetLiveCasinoBetSetting(int custId, string language);

        IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency);

        IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting);

        ThirdPartyBetSetting GetThirdPartyBetSetting(int custId, int productid);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        void RacingUpdatingProcess(string uspName, CommonBetSetting racingBetSetting);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting data, string group);

        IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IResultInfo UpdateGoldDeluxeBetSetting(CommonBetSetting data, string group);

        IResultInfo UpdateGoldDeluxeStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, string xmlData);

        IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip);

        void UpdateRacing15131(CommonBetSetting racingBetSetting);

        void UpdateRacing15141(CommonBetSetting racingBetSetting);

        void UpdateRacing15151(CommonBetSetting racingBetSetting);

        void UpdateRacing15241(CommonBetSetting racingBetSetting);

        void UpdateRacing15251(CommonBetSetting racingBetSetting);

        void UpdateRacing15351(CommonBetSetting racingBetSetting);

        IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList);

        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting);

        IResultInfo UpdateThirdPartyProductStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, string xmlData);
    }
}