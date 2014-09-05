// -----------------------------------------------------------------------
// <copyright file="IBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Customer
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Customer.BetSetting;
    using Fanex.Athena.Models.DataDomain;

    /// <summary>
    ///     IBet setting service.
    /// </summary>
    public interface IBetSettingService
    {
        AgCasinoBetSetting GetAgCasinoSetting(int custId);

        IList<AgCasinoGame> GetAgCasinoGroupBettingLimit(int custId, string siteName, int currency);

        IList<BetSetting> GetKenoBetSetting(int custId);

        IList<BetSetting> GetSportBookBetSetting(int custId, out EsportSettingType esportSetting);

        IList<BetSetting> GetNumberGameBetSetting(int custId);

        IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        IList<LiveCasinoBetSettingViewModel> GetLiveCasinoBetSetting(int custId, string language);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, IList<KenoBetSetting> betSettings);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isEnable);

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

        IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip);

        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip, string username);
    }
}