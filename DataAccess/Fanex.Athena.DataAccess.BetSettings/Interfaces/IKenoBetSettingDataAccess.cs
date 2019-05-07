// -----------------------------------------------------------------------
// <copyright file="IKenoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IKenoBetSettingDataAccess
    {
        Task<IList<BetSetting>> GetBetSettingsAsync(int custId);

        Task<IResultInfo> UpdateBetSettingAsync(int custId, int userId, string language, string ip, string xmlData);
    }
}