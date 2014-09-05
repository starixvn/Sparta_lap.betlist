// -----------------------------------------------------------------------
// <copyright file="IBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Interface.Customer
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;

    /// <summary>
    ///     Bet setting interface.
    /// </summary>
    public interface IBetSettingDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting);

        IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting data, string group);

        AgCasinoBetSetting GetAgCasinoSetting(int custId);

        IList<BetSetting> GetNumberGameBetSetting(int custId);

        IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        IList<LiveCasinoBetSetting> GetLiveCasinoBetSetting(int custId, string language);

        IList<BetSetting> GetKenoBetSetting(int custId);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList);

        IResultInfo UpdateNumbetGameBetSetting(NumberGameBetSetting param);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, string xmlData);

        IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, string xmlData);
    }
}