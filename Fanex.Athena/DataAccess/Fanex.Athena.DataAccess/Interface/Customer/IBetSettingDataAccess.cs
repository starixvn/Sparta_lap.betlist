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
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;

    /// <summary>
    ///     Bet setting interface.
    /// </summary>
    public interface IBetSettingDataAccess
    {
        IList<BetSetting> GetSportBookBetSetting(int custId);

        IList<BetSetting> GetNumberGameBetSetting(int custId);

        IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency);

        IList<BetSetting> GetVirtualSportsBetSetting(int custId);

        IList<LiveCasinoBetSetting> GetLiveCasinoBetSetting(int custId);

        IList<BetSetting> GetKenoBetSetting(int custId);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting common, IDictionary<string, object> setting);

        IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList);

        IResultInfo UpdateNumbetGameBetSetting(NumberGameBetSetting param);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, string xmlData);

        IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateKenoBetSetting(CommonBetSetting common, IDictionary<string, object> setting);
    }
}