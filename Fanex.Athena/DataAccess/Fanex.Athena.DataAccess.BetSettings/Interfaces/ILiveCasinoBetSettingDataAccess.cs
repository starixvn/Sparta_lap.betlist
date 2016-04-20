// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.BetSettings;

    public interface ILiveCasinoBetSettingDataAccess
    {
        IList<LiveCasinoBetSetting> GetBetSetting(int custId, string language);

        IResultInfo UpdateBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip);
    }
}