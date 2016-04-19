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
    using Fanex.Athena.Models.DTO.Common;

    public interface IBetSettingDataAccess
    {
        IList<BetSetting> GetKenoBetSettings(int custId);

        IList<LiveCasinoBetSetting> GetLiveCasinoBetSetting(int custId, string language);

        IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, string xmlData);

        IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip);

        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, string xmlData);
    }
}