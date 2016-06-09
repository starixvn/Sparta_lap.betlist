// -----------------------------------------------------------------------
// <copyright file="ISportsbookBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ISportsbookBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId, ref EsportSettingType esportSetting);

        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IResultInfo UpdateBetSetting(CommonBetSetting data, IDictionary<string, object> setting);
    }
}