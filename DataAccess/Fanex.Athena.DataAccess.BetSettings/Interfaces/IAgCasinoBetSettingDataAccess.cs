// -----------------------------------------------------------------------
// <copyright file="IAgCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.DTO.AgCasino;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IAgCasinoBetSettingDataAccess
    {
        Task<AgCasinoBetSetting> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateStatusAsync(int userId, int custId, string language, string ip, bool isEnabled);

        Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting data, string group);
    }
}