// -----------------------------------------------------------------------
// <copyright file="INumberGameBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface INumberGameBetSettingDataAccess
    {
        Task<IList<BetSetting>> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateBetSettingAsync(NumberGameBetSetting param);
    }
}