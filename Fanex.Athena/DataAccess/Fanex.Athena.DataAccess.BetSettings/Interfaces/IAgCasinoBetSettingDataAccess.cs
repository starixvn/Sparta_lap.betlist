// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;

    public interface IAgCasinoBetSettingDataAccess
    {
        AgCasinoBetSetting GetBetSetting(int custId);

        IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled);

        IResultInfo UpdateBetSetting(CommonBetSetting data, string group);
    }
}