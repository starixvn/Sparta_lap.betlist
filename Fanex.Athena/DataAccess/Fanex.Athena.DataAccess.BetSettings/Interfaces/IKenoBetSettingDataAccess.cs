// -----------------------------------------------------------------------
// <copyright file="IKenoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;

    public interface IKenoBetSettingDataAccess
    {
        IList<BetSetting> GetBetSettings(int custId);

        IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, string xmlData);
    }
}