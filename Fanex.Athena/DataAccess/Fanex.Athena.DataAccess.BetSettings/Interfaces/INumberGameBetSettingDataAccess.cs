// -----------------------------------------------------------------------
// <copyright file="INumberGameBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.BetSettings;

    public interface INumberGameBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId);

        IResultInfo UpdateBetSetting(NumberGameBetSetting param);
    }
}