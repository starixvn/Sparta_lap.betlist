// -----------------------------------------------------------------------
// <copyright file="IVirtualSportsBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVirtualSportsBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId);

        IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, string xmlData);
    }
}