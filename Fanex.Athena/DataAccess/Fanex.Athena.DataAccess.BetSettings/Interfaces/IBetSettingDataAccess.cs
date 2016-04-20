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
        IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting);
    }
}